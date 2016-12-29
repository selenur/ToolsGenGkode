using System;
using System.Collections.Generic;

namespace ELW.Library.Math.Expressions {
    /// <summary>
    /// Type of compiled expression item's content.
    /// </summary>
    public enum CompiledExpressionItemKind {
        Constant = 1,
        Variable = 2,
        Operation = 3
    }

    /// <summary>
    /// Represents a compiled expression, which can be used for calculating.
    /// </summary>
    public sealed class CompiledExpression {
        private readonly List<CompiledExpressionItem> compiledExpressionItems;

        public List<CompiledExpressionItem> CompiledExpressionItems {
            get {
                return compiledExpressionItems;
            }
        }

        public CompiledExpression(List<CompiledExpressionItem> compiledExpressionItems) {
            this.compiledExpressionItems = compiledExpressionItems;
        }
    }

    /// <summary>
    /// Item of compiled expression.
    /// </summary>
    public sealed class CompiledExpressionItem {
        private readonly CompiledExpressionItemKind kind;
        public CompiledExpressionItemKind Kind {
            get {
                return kind;
            }
        }

        private readonly double constant;
        public double Constant {
            get {
                if (kind != CompiledExpressionItemKind.Constant)
                    throw new InvalidOperationException("Type mismatch.");
                return constant;
            }
        }

        private readonly string variableName;
        public string VariableName {
            get {
                if (kind != CompiledExpressionItemKind.Variable)
                    throw new InvalidOperationException("Type mismatch.");
                return variableName;
            }
        }

        private readonly string operationName;
        public string OperationName {
            get {
                if (kind != CompiledExpressionItemKind.Operation)
                    throw new InvalidOperationException("Type mismatch.");
                return operationName;
            }
        }

        public CompiledExpressionItem(CompiledExpressionItemKind kind, object value) {
            if (value == null)
                throw new ArgumentNullException("value");
            //
            this.kind = kind;
            switch (kind) {
                case CompiledExpressionItemKind.Constant: {
                    constant = (double) value;
                    break;
                }
                case CompiledExpressionItemKind.Variable: {
                    variableName = (string) value;
                    break;
                }
                case CompiledExpressionItemKind.Operation: {
                    operationName = (string) value;
                    break;
                }
                default: {
                    throw new InvalidOperationException("Unexpected kind.");
                }
            }
        }

        public override string ToString() {
            return String.Format("{0} {1}", kind,
                kind == CompiledExpressionItemKind.Constant ? constant.ToString() :
                kind == CompiledExpressionItemKind.Operation ? operationName : variableName);
        }
    }
}