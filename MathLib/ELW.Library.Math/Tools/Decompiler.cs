using System;
using System.Collections.Generic;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;

namespace ELW.Library.Math.Tools {
    /// <summary>
    /// Represents a partial result of decompiling a compiled expression items sequence.
    /// </summary>
    internal sealed class DecompiledExpressionItem {
        private readonly Operation lastOperation;
        public Operation LastOperation {
            get {
                if (!isComplex)
                    throw new InvalidOperationException("This is not defined because item is not complex.");
                return lastOperation;
            }
        }

        private readonly PreparedExpression expression;
        public PreparedExpression Expression {
            get {
                return expression;
            }
        }

        private readonly bool isComplex;
        /// <summary>
        /// "a+5" - complex, "45" - not complex.
        /// </summary>
        public bool IsComplex {
            get {
                return isComplex;
            }
        }

        public DecompiledExpressionItem(bool isComplex, PreparedExpression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");
            //
            this.isComplex = isComplex;
            this.expression = expression;
        }

        public DecompiledExpressionItem(bool isComplex, PreparedExpression expression, Operation lastOperation)
            : this(isComplex, expression) {
            //
            if (lastOperation == null)
                throw new ArgumentNullException("lastOperation");
            //
            this.lastOperation = lastOperation;
        }
    }

    /// <summary>
    /// Implements decompiler logic for specified compiled expressions.
    /// </summary>
    public sealed class Decompiler {
        private readonly OperationsRegistry operationsRegistry;
        public OperationsRegistry OperationsRegistry {
            get {
                return operationsRegistry;
            }
        }

        public Decompiler(OperationsRegistry operationsRegistry) {
            if (operationsRegistry == null)
                throw new ArgumentNullException("operationsRegistry");
            //
            this.operationsRegistry = operationsRegistry;
        }

        public PreparedExpression Decompile(CompiledExpression compiledExpression) {
            if (compiledExpression == null)
                throw new ArgumentNullException("compiledExpression");
            //
            List<DecompiledExpressionItem> decompilationStack = new List<DecompiledExpressionItem>();
            //
            for (int i = 0; i < compiledExpression.CompiledExpressionItems.Count; i++) {
                CompiledExpressionItem item = compiledExpression.CompiledExpressionItems[i];
                //
                switch (item.Kind) {
                    case CompiledExpressionItemKind.Constant: {
                        List<PreparedExpressionItem> items = new List<PreparedExpressionItem>();
                        items.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Constant, item.Constant));
                        decompilationStack.Add(new DecompiledExpressionItem(false, new PreparedExpression(items)));
                        break;
                    }
                    case CompiledExpressionItemKind.Variable: {
                        List<PreparedExpressionItem> items = new List<PreparedExpressionItem>();
                        items.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Variable, item.VariableName));
                        decompilationStack.Add(new DecompiledExpressionItem(false, new PreparedExpression(items)));
                        break;
                    }
                    case CompiledExpressionItemKind.Operation: {
                        Operation operation = operationsRegistry.GetOperationByName(item.OperationName);
                        List<PreparedExpressionItem> resultExpression = new List<PreparedExpressionItem>();
                        // Begining to construct a new expression string
                        if (operation.Kind == OperationKind.Function) {
                            resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Signature, operation.Signature[0]));
                            resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, DelimiterKind.OpeningBrace));
                        } else if (operation.OperandsCount == 1) {
                            // Unary operator
                            resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Signature, operation.Signature[0]));
                        }
                        // For each argument we have to determine, need there are braces or not
                        for (int j = 0; j < operation.OperandsCount; j++) {
                            int index = decompilationStack.Count - operation.OperandsCount + j;
                            if (index < 0)
                                throw new MathProcessorException("Stack is empty.");
                            //
                            DecompiledExpressionItem decompiledItem = decompilationStack[index];
                            bool applyBraces = false;
                            if (operation.Kind != OperationKind.Function) {
                                // First argument
                                if (j == 0) {
                                    if (decompiledItem.IsComplex)
                                        if (decompiledItem.LastOperation.Kind != OperationKind.Function) {
                                            if (decompiledItem.LastOperation.OperandsCount == 1)
                                                applyBraces = true;
                                            else {
                                                if (operation.Priority < decompiledItem.LastOperation.Priority)
                                                    applyBraces = true;
                                                else if (operation.Priority == decompiledItem.LastOperation.Priority) {
                                                    if (operationsRegistry.GetAssociationByPriority(operation.Priority) == PriorityAssociation.RightAssociated)
                                                        applyBraces = true;
                                                }
                                            }
                                        }
                                } else
                                    // Last argument
                                    if (j == operation.OperandsCount - 1)
                                        if (decompiledItem.IsComplex)
                                            if (decompiledItem.LastOperation.Kind != OperationKind.Function) {
                                                if (decompiledItem.LastOperation.OperandsCount == 1)
                                                    applyBraces = true;
                                                else {
                                                    if (operation.Priority < decompiledItem.LastOperation.Priority)
                                                        applyBraces = true;
                                                    else if (operation.Priority == decompiledItem.LastOperation.Priority) {
                                                        if (operationsRegistry.GetAssociationByPriority(operation.Priority) == PriorityAssociation.LeftAssociated)
                                                            applyBraces = true;
                                                    }
                                                }
                                            }
                            }
                            if (applyBraces)
                                resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, DelimiterKind.OpeningBrace));
                            resultExpression.AddRange(decompiledItem.Expression.PreparedExpressionItems);
                            if (applyBraces)
                                resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, DelimiterKind.ClosingBrace));
                            
                            
                            // Appending delimiters between arguments
                            if (j < operation.OperandsCount - 1) {
                                if (operation.Kind == OperationKind.Function) {
                                    resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, DelimiterKind.Comma));
                                } else {
                                    resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Signature, operation.Signature[j]));
                                }
                            }
                        }
                        decompilationStack.RemoveRange(decompilationStack.Count - operation.OperandsCount, operation.OperandsCount);
                        //
                        if (operation.Kind == OperationKind.Function)
                            resultExpression.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, DelimiterKind.ClosingBrace));
                        //
                        decompilationStack.Add(new DecompiledExpressionItem(true, new PreparedExpression(resultExpression), operation));
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unknown item kind.");
                    }
                }
            }
            //
            if (decompilationStack.Count != 1)
                throw new MathProcessorException("Stack disbalance. Expression has invalid syntax.");
            return (decompilationStack[0].Expression);
        }
    }
}