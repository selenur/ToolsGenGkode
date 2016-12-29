using System;
using System.Collections.Generic;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;

namespace ELW.Library.Math.Tools {
    /// <summary>
    /// Contains a concrete value for specified variable.
    /// </summary>
    public sealed class VariableValue {
        public double value;
        public double Value {
            get {
                return value;
            }
           
        }

        private readonly string variableName;
        public string VariableName {
            get {
                return variableName;
            }
        }

        public VariableValue(double value, string variableName) {
            if (variableName == null)
                throw new ArgumentNullException("variableName");
            if (variableName.Length == 0)
                throw new ArgumentException("String is empty.", "variableName");
            //
            this.value = value;
            this.variableName = variableName;
        }
    }

    /// <summary>
    /// Implements a simple calculator calculates compiled expressions.
    /// </summary>
    public sealed class Calculator {
        private readonly OperationsRegistry operationsRegistry;
        public OperationsRegistry OperationsRegistry {
            get {
                return operationsRegistry;
            }
        }

        public Calculator(OperationsRegistry operationsRegistry) {
            if (operationsRegistry == null)
                throw new ArgumentNullException("operationsRegistry");
            //
            this.operationsRegistry = operationsRegistry;
        }

        public double Calculate(CompiledExpression compiledExpression, List<VariableValue> variableValues) {
            if (compiledExpression == null)
                throw new ArgumentNullException("compiledExpression");
            if (variableValues == null)
                throw new ArgumentNullException("variableValues");
            //
            List<double> calculationsStack = new List<double>();
            //
            for (int i = 0; i < compiledExpression.CompiledExpressionItems.Count; i++) {
                CompiledExpressionItem item = compiledExpression.CompiledExpressionItems[i];
                //
                switch (item.Kind) {
                    case CompiledExpressionItemKind.Constant: {
                        calculationsStack.Add(item.Constant);
                        break;
                    }
                    case CompiledExpressionItemKind.Variable: {
                        // TODO: Add dictionary optimizations.
                        bool variableValueFound = false;
                        foreach (VariableValue variableValue in variableValues) {
                            if (item.VariableName == variableValue.VariableName) {
                                variableValueFound = true;
                                calculationsStack.Add(variableValue.Value);
                                break;
                            }
                        }
                        if (!variableValueFound)
                            throw new MathProcessorException(String.Format("Variable {0} is not initialized.", item.VariableName));
                        break;
                    }
                    case CompiledExpressionItemKind.Operation: {
                        Operation operation = operationsRegistry.GetOperationByName(item.OperationName);
                        double[] parametersArray = new double[operation.OperandsCount];
                        //
                        if (calculationsStack.Count - operation.OperandsCount < 0)
                            throw new MathProcessorException("Stack is empty.");
                        for (int j = 0; j < operation.OperandsCount; j++) {
                            int index = calculationsStack.Count - operation.OperandsCount + j;
                            parametersArray[j] = calculationsStack[index];
                        }
                        //
                        calculationsStack.RemoveRange(calculationsStack.Count - operation.OperandsCount, operation.OperandsCount);
                        calculationsStack.Add(operation.Calculator.Calculate(parametersArray));
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unknown item kind.");
                    }
                }
            }
            //
            if (calculationsStack.Count != 1)
                throw new MathProcessorException("Stack disbalance. Expression has invalid syntax.");
            return calculationsStack[0];
        }
    }
}