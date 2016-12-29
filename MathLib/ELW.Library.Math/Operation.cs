using System;
using ELW.Library.Math.Calculators;

namespace ELW.Library.Math {
    /// <summary>
    /// Type of operation.
    /// </summary>
    public enum OperationKind {
        Operator = 1,
        Function = 2
    }

    /// <summary>
    /// Association direction of priority.
    /// </summary>
    public enum PriorityAssociation {
        /// <summary>
        /// For example, normal binary subtraction or multiplication.
        /// </summary>
        LeftAssociated = 1,
        /// <summary>
        /// For example, ternary ?: operator.
        /// </summary>
        RightAssociated = 2
    }

    /// <summary>
    /// Represents an operation with calculator associated.
    /// </summary>
    public sealed class Operation {
        private readonly string name;
        /// <summary>
        /// Operation name - unique string.
        /// </summary>
        public string Name {
            get {
                return name;
            }
        }

        private readonly OperationKind kind;
        /// <summary>
        /// Kind of operation.
        /// </summary>
        public OperationKind Kind {
            get {
                return kind;
            }
        }

        private readonly int priority;
        /// <summary>
        /// Integer priority of operation.
        /// </summary>
        public int Priority {
            get {
                return priority;
            }
        }

        private readonly string[] signature;
        /// <summary>
        /// Set of signature strings.
        /// </summary>
        public string[] Signature {
            get {
                return signature;
            }
        }

        private readonly int operandsCount;
        /// <summary>
        /// Count of operands.
        /// </summary>
        public int OperandsCount {
            get {
                return operandsCount;
            }
        }

        private readonly IOperationCalculator calculator;
        /// <summary>
        /// Calculator for this operation.
        /// </summary>
        public IOperationCalculator Calculator {
            get {
                return calculator;
            }
        }

        public Operation(string name, OperationKind kind, string[] signature, int operandsCount, IOperationCalculator calculator, int priority) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("Empty name.", "name");
            if (signature == null)
                throw new ArgumentNullException("signature");
            if (signature.Length == 0)
                throw new ArgumentException("Signature is empty.", "signature");
            if (calculator == null)
                throw new ArgumentNullException("calculator");
            if ((kind == OperationKind.Operator) && (operandsCount > 1) && (signature.Length != operandsCount - 1))
                throw new ArgumentException("Invalid array length.", "signature");
            if ((kind == OperationKind.Function) && (signature.Length != 1))
                throw new InvalidOperationException("Signature of function must contain one string item.");
            //
            if (kind == OperationKind.Operator)
                this.priority = priority;
            this.kind = kind;
            this.name = name;
            this.calculator = calculator;
            this.operandsCount = operandsCount;
            this.signature = signature;
        }

        public Operation(string name, OperationKind kind, string[] signature, int operandsCount, IOperationCalculator calculator)
            : this(name, kind, signature, operandsCount, calculator, 0) {
            //
        }

        public override string ToString() {
            return String.Format("{0} {1}", kind, name);
        }
    }
}