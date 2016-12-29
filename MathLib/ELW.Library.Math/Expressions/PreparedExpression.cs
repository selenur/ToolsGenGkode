using System;
using System.Collections.Generic;
using System.Text;

namespace ELW.Library.Math.Expressions {
    /// <summary>
    /// Delimiters supported.
    /// </summary>
    public enum DelimiterKind {
        OpeningBrace = 1,
        ClosingBrace = 2,
        Comma = 3
    }

    /// <summary>
    /// Type of prepared expression item content.
    /// </summary>
    public enum PreparedExpressionItemKind {
        /// <summary>
        /// Constant value.
        /// </summary>
        Constant,
        /// <summary>
        /// Variable name.
        /// </summary>
        Variable,
        /// <summary>
        /// Braces, commas.
        /// </summary>
        Delimiter,
        /// <summary>
        /// Registered signature.
        /// </summary>
        Signature
    }

    /// <summary>
    /// Represents a prepared sequence of precompiled items for compiling.
    /// </summary>
    public sealed class PreparedExpression {
        public readonly List<PreparedExpressionItem> preparedExpressionItems;
        public List<PreparedExpressionItem> PreparedExpressionItems {
            get {
                return preparedExpressionItems;
            }
        }

        public PreparedExpression(List<PreparedExpressionItem> preparedExpressionItems) {
            this.preparedExpressionItems = preparedExpressionItems;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PreparedExpressionItem item in preparedExpressionItems) {
                switch (item.Kind) {
                    case PreparedExpressionItemKind.Constant: {
                        stringBuilder.Append(item.Constant.ToString());
                        break;
                    }
                    case PreparedExpressionItemKind.Delimiter: {
                        switch (item.DelimiterKind) {
                            case DelimiterKind.OpeningBrace: {
                                stringBuilder.Append("(");
                                break;
                            }
                            case DelimiterKind.ClosingBrace: {
                                stringBuilder.Append(")");
                                break;
                            }
                            case DelimiterKind.Comma: {
                                stringBuilder.Append(",");
                                break;
                            }
                            default: {
                                throw new InvalidOperationException("Unknown delimiter kind.");
                            }
                        }
                        break;
                    }
                    case PreparedExpressionItemKind.Variable: {
                        stringBuilder.Append(item.VariableName);
                        break;
                    }
                    case PreparedExpressionItemKind.Signature: {
                        stringBuilder.Append(item.Signature);
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unknown item kind.");
                    }
                }
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// Represents a part of parsed expression or decompiled expression.
    /// </summary>
    public sealed class PreparedExpressionItem {
        private readonly PreparedExpressionItemKind kind;
        public PreparedExpressionItemKind Kind {
            get {
                return kind;
            }
        }

        private readonly double constant;
        public double Constant {
            get {
                return constant;
            }
        }

        private readonly string variableName;
        public string VariableName {
            get {
                return variableName;
            }
        }

        private readonly DelimiterKind delimiterKind;
        public DelimiterKind DelimiterKind {
            get {
                return delimiterKind;
            }
        }

        private readonly string signature;
        public string Signature {
            get {
                return signature;
            }
        }

        public PreparedExpressionItem(PreparedExpressionItemKind kind, object value) {
            if (value == null)
                throw new ArgumentNullException("value");
            //
            this.kind = kind;
            switch (kind) {
                case PreparedExpressionItemKind.Constant: {
                    constant = (double) value;
                    break;
                }
                case PreparedExpressionItemKind.Variable: {
                    variableName = (string) value;
                    break;
                }
                case PreparedExpressionItemKind.Delimiter: {
                    delimiterKind = (DelimiterKind) value;
                    break;
                }
                case PreparedExpressionItemKind.Signature: {
                    signature = (string) value;
                    break;
                }
                default: {
                    throw new InvalidOperationException("Unexpected item kind.");
                }
            }
        }

        public override string ToString() {
            return String.Format("PreparedExpressionItem {0}", kind);
        }
    }
}