using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using ELW.Library.Math.Calculators;

namespace ELW.Library.Math {
    /// <summary>
    /// Supported operations and configuration manager.
    /// </summary>
    public sealed class OperationsRegistry {
        private readonly List<Operation> operationsList = new List<Operation>();
        private readonly Dictionary<int, PriorityAssociation> priorityAssociationsDictionary = new Dictionary<int, PriorityAssociation>();
        private readonly Dictionary<string, Operation> operationNamesDictionary = new Dictionary<string, Operation>();
        private readonly Dictionary<string, ICollection<Operation>> operationSignaturesDictionary = new Dictionary<string, ICollection<Operation>>();

        public bool IsPriorityDefined(int priority) {
            return priorityAssociationsDictionary.ContainsKey(priority);
        }

        public PriorityAssociation GetAssociationByPriority(int priority) {
            if (!IsPriorityDefined(priority))
                throw new ArgumentException("Specified priority is not defined.", "priority");
            return priorityAssociationsDictionary[priority];
        }
        
        public bool IsOperationDefined(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("String is empty.", "name");
            //
            return operationNamesDictionary.ContainsKey(name);
        }

        public Operation GetOperationByName(string name) {
            if (!IsOperationDefined(name))
                throw new ArgumentException("No operation defined with name specified.", "name");
            return operationNamesDictionary[name];
        }

        public bool IsSignatureDefined(string signature) {
            if (signature == null)
                throw new ArgumentNullException("signature");
            if (signature.Length == 0)
                throw new ArgumentException("String is empty.", "signature");
            //
            return operationSignaturesDictionary.ContainsKey(signature);
        }

        public ICollection<Operation> GetOperationsUsingSignature(string signature) {
            if (!IsSignatureDefined(signature))
                throw new ArgumentException("No operation uses signature specified.", "signature");
            //
            return operationSignaturesDictionary[signature];
        }

        private readonly int[] signaturesLens;
        public int[] SignaturesLens {
            get {
                return signaturesLens;
            }
        }

        #region Nested

        /// <summary>
        /// Used for sort while deserializing configuration stream.
        /// </summary>
        private sealed class SerializedSignature {
            private readonly string signature;
            public string Signature {
                get {
                    return signature;
                }
            }

            private readonly int number;
            public int Number {
                get {
                    return number;
                }
            }

            public static int Compare(SerializedSignature a, SerializedSignature b) {
                if (a == null) {
                    if (b == null)
                        return 0;
                    return -1;
                }
                if (b == null)
                    return 1;
                if (a.Number > b.Number)
                    return 1;
                if (a.Number < b.Number)
                    return -1;
                return 0;
            }

            public SerializedSignature(string signature, int number) {
                if (signature == null)
                    throw new ArgumentNullException("signature");
                //
                this.signature = signature;
                this.number = number;
            }
        }

        #endregion

        /// <summary>
        /// Load configuration from embedded resource xml.
        /// </summary>
        private void initializeFromConfigurationXml() {
            const string documentPath = "Operations.xml";
            const string schemaPath = "Operations.xsd";
            //
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            //
            using (Stream stream = assembly.GetManifestResourceStream(assemblyName.Name + "." + documentPath)) {
                if (stream == null)
                    throw new InvalidOperationException(String.Format("Could not load '{0}' configuration resource from assembly.", documentPath));
                //
                XmlDocument document = new XmlDocument();
                document.Load(stream);

                // Validating schema
                XmlSchema schema;
                using (Stream streamSchema = assembly.GetManifestResourceStream(assemblyName.Name + "." + schemaPath)) {
                    if (streamSchema == null)
                        throw new InvalidOperationException("Could not load schema resource from assembly.");
                    schema = XmlSchema.Read(streamSchema, validationSchema);
                }

                document.Schemas.Add(schema);
                document.Validate(documentValidation);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
                namespaceManager.AddNamespace("main", schema.TargetNamespace);

                XmlNodeList prioritiesNodeList = document.SelectNodes("main:operations/main:priorities/main:priority", namespaceManager);
                if (prioritiesNodeList == null)
                    throw new InvalidOperationException("Could not load priorities from configuration.");
                //
                foreach (XmlNode priorityNode in prioritiesNodeList) {
                    int priorityValue = Convert.ToInt32(priorityNode.Attributes["value"].Value);
                    PriorityAssociation priorityAssociation = (priorityNode.Attributes["association"].Value == "left") ?
                        PriorityAssociation.LeftAssociated : PriorityAssociation.RightAssociated;
                    priorityAssociationsDictionary.Add(priorityValue, priorityAssociation);
                }

                XmlNodeList operationsNodeList = document.SelectNodes("main:operations/main:operations/main:operation", namespaceManager);
                if (operationsNodeList == null)
                    throw new InvalidOperationException("Could not load operations from configuration.");
                //
                foreach (XmlNode operationNode in operationsNodeList) {
                    string operationName = operationNode.Attributes["name"].Value;
                    int operationOperandsCount = Convert.ToInt32(operationNode.Attributes["operands"].Value);
                    OperationKind operationKind = (operationNode.Attributes["kind"].Value == "operator") ?
                        OperationKind.Operator : OperationKind.Function;
                    int operationPriority;
                    if (operationKind == OperationKind.Operator)
                        operationPriority = Convert.ToInt32(operationNode.Attributes["priority"].Value);
                    else
                        operationPriority = 0;
                    //
                    List<SerializedSignature> signaturesOrderedList = new List<SerializedSignature>();
                    XmlNode signaturesNode = operationNode["signatures"];
                    if (signaturesNode == null)
                        throw new InvalidOperationException("No signatures found during parsing operation.");
                    foreach (XmlNode signatureNode in signaturesNode.ChildNodes) {
                        signaturesOrderedList.Add(new SerializedSignature(
                            signatureNode.Attributes["value"].Value,
                            Int32.Parse(signatureNode.Attributes["number"].Value)));
                    }
                    signaturesOrderedList.Sort(new Comparison<SerializedSignature>(SerializedSignature.Compare));
                    string[] operationSignatures = new string[signaturesOrderedList.Count];
                    for (int i = 0; i < signaturesOrderedList.Count; i++)
                        operationSignatures[i] = signaturesOrderedList[i].Signature;
                    //
                    XmlNode calculatorNode = operationNode["calculator"];
                    if (calculatorNode == null)
                        throw new InvalidOperationException("No calculator specified.");
                    string calculatorType = calculatorNode.Attributes["type"].Value;
                    if (String.IsNullOrEmpty(calculatorType))
                        throw new InvalidOperationException("Empty calculator type string.");
                    string[] calculatorTypeParts = calculatorType.Split(',');
                    if (calculatorTypeParts.Length != 2)
                        throw new InvalidOperationException("Invalid type declarations syntax.");
                    string calculatorTypeName = calculatorTypeParts[0];
                    string calculatorAssemblyName = calculatorTypeParts[1];
                    //
                    Assembly calculatorAssembly = Assembly.Load(calculatorAssemblyName);
                    IOperationCalculator operationCalculator = (IOperationCalculator) calculatorAssembly.CreateInstance(calculatorTypeName, false);
                    //
                    if (operationKind == OperationKind.Operator)
                        operationsList.Add(new Operation(operationName, OperationKind.Operator,
                            operationSignatures, operationOperandsCount, operationCalculator,
                            operationPriority));
                    else {
                        operationsList.Add(new Operation(operationName, OperationKind.Function,
                            operationSignatures, operationOperandsCount, operationCalculator));
                    }
                }
            }
        }

        private static void documentValidation(object sender, ValidationEventArgs e) {
            if (e.Exception != null) {
                throw new InvalidOperationException("Invalid configuration document.", e.Exception);
            }
        }

        private static void validationSchema(object sender, ValidationEventArgs e) {
            if (e.Exception != null) {
                throw new InvalidOperationException("Invalid configuration schema.", e.Exception);
            }
        }

        public OperationsRegistry() {
            initializeFromConfigurationXml();
            // Storing signatures lenghts has been met during processing
            List<int> lens = new List<int>();
            foreach (Operation operation in operationsList) {
                operationNamesDictionary.Add(operation.Name, operation);
                //
                foreach (string s in operation.Signature) {
                    if (!operationSignaturesDictionary.ContainsKey(s))
                        operationSignaturesDictionary.Add(s, new List<Operation>());
                    operationSignaturesDictionary[s].Add(operation);
                }
                // Add signature lenght if not added already
                foreach (string s in operation.Signature) {
                    int len = s.Length;
                    bool alreadySaved = false;
                    foreach (int i in lens) {
                        if (i == len) {
                            alreadySaved = true;
                            break;
                        }
                    }
                    if (!alreadySaved)
                        lens.Add(len);
                }
            }
            lens.Sort();
            signaturesLens = new int[lens.Count];
            lens.CopyTo(signaturesLens);
        }
    }
}
