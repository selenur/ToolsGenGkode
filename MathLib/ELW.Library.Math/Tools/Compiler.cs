using System;
using System.Collections.Generic;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;

namespace ELW.Library.Math.Tools {
    /// <summary>
    /// Implements compiler logic.
    /// </summary>
    public sealed class Compiler {
        private readonly OperationsRegistry operationsRegistry;
        public OperationsRegistry OperationsRegistry {
            get {
                return operationsRegistry;
            }
        }

        public Compiler(OperationsRegistry operationsRegistry) {
            if (operationsRegistry == null)
                throw new ArgumentNullException("operationsRegistry");
            //
            this.operationsRegistry = operationsRegistry;
        }

        /// <summary>
        /// Returns a compiled expression for specified source string.
        /// </summary>
        public CompiledExpression Compile(PreparedExpression preparedExpression) {
            if (preparedExpression == null)
                throw new ArgumentNullException("preparedExpression");
            //
            OperationsStack operationsStack = new OperationsStack(operationsRegistry);
            //
            for (int itemIndex = 0; itemIndex < preparedExpression.PreparedExpressionItems.Count; itemIndex++) {
                PreparedExpressionItem item = preparedExpression.PreparedExpressionItems[itemIndex];
                // If constant or variable - add to result
                if (item.Kind == PreparedExpressionItemKind.Constant)
                    operationsStack.PushConstant(item.Constant);
                if (item.Kind == PreparedExpressionItemKind.Variable)
                    operationsStack.PushVariable(item.VariableName);
                // If delimiter
                if (item.Kind == PreparedExpressionItemKind.Delimiter) {
                    operationsStack.PushDelimiter(item.DelimiterKind);
                }
                // Signature (operator signature / part of signature / function)
                if (item.Kind == PreparedExpressionItemKind.Signature) {
                    List<Operation> operations = new List<Operation>(operationsRegistry.GetOperationsUsingSignature(item.Signature));
                    operations.Sort(new Comparison<Operation>(compareOperationsByOperandsCount));
                    //
                    for (int i = 0; i < operations.Count; i++) {
                        Operation operation = operations[i];
                        // Operator
                        if (operation.Kind == OperationKind.Operator) {
                            // Unary operator
                            if (operation.OperandsCount == 1) {
                                // If operator placed at the start of subexpression
                                if ((itemIndex == 0) ||
                                    ((itemIndex > 0) && (preparedExpression.PreparedExpressionItems[itemIndex - 1].Kind == PreparedExpressionItemKind.Delimiter) && (preparedExpression.PreparedExpressionItems[itemIndex - 1].DelimiterKind == DelimiterKind.OpeningBrace))) {
                                    //
                                    operationsStack.PushUnaryOperator(operation);
                                    break;
                                }
                            }
                            // Binary operator
                            if (operation.OperandsCount == 2) {
                                operationsStack.PushBinaryOperator(operation);
                                break;
                            }
                            // Ternary and more
                            if (operation.OperandsCount > 2) {
                                int partNumber = 0;
                                for (int k = 0; k < operation.Signature.Length; k++) {
                                    if (operation.Signature[k] == item.Signature) {
                                        partNumber = k + 1;
                                        break;
                                    }
                                }
                                // If it is start part in signature
                                if (partNumber == 1) {
                                    operationsStack.PushComplexOperatorFirstSignature(operation);
                                    break;
                                }
                                //
                                operationsStack.PushComplexOperatorNonFirstSignature(operation, partNumber);
                                break;
                            }
                        }
                        // Function
                        if (operation.Kind == OperationKind.Function) {
                            operationsStack.PushFunction(operation);
                            break;
                        }
                    }
                }
            }
            // 
            operationsStack.DoFinalFlush();
            //
            CompiledExpression res = operationsStack.GetResult();
            if (!isCompiledExpressionStackBalanced(res))
                throw new CompilerSyntaxException("Operands disbalance detected.");
            return res;
        }

        /// <summary>
        /// Comparison.
        /// </summary>
        private static int compareOperationsByOperandsCount(Operation x, Operation y) {
            if (x == null) {
                if (y == null)
                    return 0;
                return -1;
            }
            if (y == null)
                return 1;
            //
            if (x.OperandsCount > y.OperandsCount)
                return 1;
            if (x.OperandsCount < y.OperandsCount)
                return -1;
            return 0;
        }

        /// <summary>
        /// Checks a compiled expression for stack balance.
        /// </summary>
        private bool isCompiledExpressionStackBalanced(CompiledExpression compiledExpression) {
            if (compiledExpression == null)
                throw new ArgumentNullException("compiledExpression");
            //
            int stackPointer = 0;
            //
            for (int i = 0; i < compiledExpression.CompiledExpressionItems.Count; i++) {
                CompiledExpressionItem item = compiledExpression.CompiledExpressionItems[i];
                //
                switch (item.Kind) {
                    case CompiledExpressionItemKind.Constant: {
                        stackPointer++;
                        break;
                    }
                    case CompiledExpressionItemKind.Variable: {
                        stackPointer++;
                        break;
                    }
                    case CompiledExpressionItemKind.Operation: {
                        Operation operation = operationsRegistry.GetOperationByName(item.OperationName);
                        stackPointer -= operation.OperandsCount - 1;
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unknown item kind.");
                    }
                }
            }
            //
            if (stackPointer != 1)
                return (false);
            return (true);
        }

        #region Nested

        /// <summary>
        /// Manages stack structure, forms a result sequence.
        /// </summary>
        private sealed class OperationsStack {
            private readonly List<CompiledExpressionItem> res = new List<CompiledExpressionItem>();
            private readonly List<OperationsStackItem> stack = new List<OperationsStackItem>();
            private readonly OperationsRegistry operationsRegistry;

            public OperationsStack(OperationsRegistry operationsRegistry) {
                if (operationsRegistry == null)
                    throw new ArgumentNullException("operationsRegistry");
                this.operationsRegistry = operationsRegistry;
            }

            public void PushConstant(double constant) {
                res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Constant, constant));
            }

            public void PushVariable(string variableName) {
                res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Variable, variableName));
            }

            public void PushDelimiter(DelimiterKind delimiterKind) {
                if (delimiterKind == DelimiterKind.OpeningBrace) {
                    stack.Add(new OperationsStackItem(OperationStackItemKind.Delimiter, DelimiterKind.OpeningBrace));
                }
                if (delimiterKind == DelimiterKind.ClosingBrace) {
                    // Pop all items before previous OpeningBrace includes it
                    int j = stack.Count - 1;
                    while (j >= 0) {
                        if ((stack[j].Kind == OperationStackItemKind.Delimiter) && (stack[j].Delimiter == DelimiterKind.OpeningBrace)) {
                            stack.RemoveAt(j);
                            break;
                        }
                        //
                        switch (stack[j].Kind) {
                            case OperationStackItemKind.Operation: {
                                res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[j].OperationName));
                                stack.RemoveAt(j);
                                break;
                            }
                            default: {
                                throw new CompilerSyntaxException("Unexpected item in stack.");
                            }
                        }
                        j--;
                    }
                    if (j < 0)
                        throw new CompilerSyntaxException("Braces syntax error.");
                    // If previous item is function - pop it
                    if (stack.Count > 0) {
                        if ((stack[stack.Count - 1].Kind == OperationStackItemKind.Operation) &&
                            (operationsRegistry.GetOperationByName(stack[stack.Count - 1].OperationName).Kind == OperationKind.Function)) {
                            //
                            res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[stack.Count - 1].OperationName));
                            stack.RemoveAt(stack.Count - 1);
                        }
                    }
                }
                if (delimiterKind == DelimiterKind.Comma) {
                    // Pop all items before previous OpeningBrace excludes it
                    int j = stack.Count - 1;
                    while (j >= 0) {
                        if ((stack[j].Kind == OperationStackItemKind.Delimiter) && (stack[j].Delimiter == DelimiterKind.OpeningBrace)) {
                            break;
                        }
                        //
                        switch (stack[j].Kind) {
                            case OperationStackItemKind.Operation: {
                                res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[j].OperationName));
                                stack.RemoveAt(j);
                                break;
                            }
                            default: {
                                throw new CompilerSyntaxException("Unexpected item in stack.");
                            }
                        }
                        j--;
                    }
                    if (j < 0)
                        throw new CompilerSyntaxException("Braces syntax error.");
                }
            }

            private void pushOperationAccordingToAssociationAndPriority(Operation operation, OperationsStackItem itemToPush) {
                // Push an operation according to association and priority
                if (stack.Count > 0) {
                    int j = stack.Count - 1;
                    bool priorityExit = false;
                    while ((j >= 0) && (!priorityExit)) {
                        if (stack[j].Kind == OperationStackItemKind.Delimiter) {
                            break;
                        }
                        //
                        switch (stack[j].Kind) {
                            case OperationStackItemKind.Operation: {
                                if (operationsRegistry.GetAssociationByPriority(operation.Priority) == PriorityAssociation.LeftAssociated) {
                                    if (operationsRegistry.GetOperationByName(stack[j].OperationName).Priority > operation.Priority)
                                        priorityExit = true;
                                } else {
                                    if (operationsRegistry.GetOperationByName(stack[j].OperationName).Priority >= operation.Priority)
                                        priorityExit = true;
                                }
                                if (!priorityExit) {
                                    res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[j].OperationName));
                                    stack.RemoveAt(j);
                                }
                                break;
                            }
                            case OperationStackItemKind.PartialSignature: {
                                priorityExit = true;
                                break;
                            }
                            default: {
                                throw new MathProcessorException("Unexpected item in stack.");
                            }
                        }
                        j--;
                    }
                }
                //
                stack.Add(itemToPush);
            }

            public void PushUnaryOperator(Operation operation) {
                pushOperationAccordingToAssociationAndPriority(operation,
                                                               new OperationsStackItem(OperationStackItemKind.Operation, operation.Name));
            }

            public void PushBinaryOperator(Operation operation) {
                pushOperationAccordingToAssociationAndPriority(operation,
                                                               new OperationsStackItem(OperationStackItemKind.Operation, operation.Name));
            }

            public void PushFunction(Operation operation) {
                stack.Add(new OperationsStackItem(OperationStackItemKind.Operation, operation.Name));
            }

            public void PushComplexOperatorFirstSignature(Operation operation) {
                PartialSignature signature = new PartialSignature();
                signature.OperationName = operation.Name;
                signature.SignaturePartNumber = 1;
                //
                pushOperationAccordingToAssociationAndPriority(operation,
                                                               new OperationsStackItem(OperationStackItemKind.PartialSignature, signature));
            }

            public void PushComplexOperatorNonFirstSignature(Operation operation, int partNumber) {
                int j = stack.Count - 1;
                while (j >= 0) {
                    if ((stack[j].Kind == OperationStackItemKind.PartialSignature) && (stack[j].PartialSignature.OperationName == operation.Name) && (stack[j].PartialSignature.SignaturePartNumber == partNumber - 1)) {
                        break;
                    }
                    //
                    switch (stack[j].Kind) {
                        case OperationStackItemKind.Operation: {
                            res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[j].OperationName));
                            stack.RemoveAt(j);
                            break;
                        }
                        default: {
                            throw new MathProcessorException("Unexpected item in stack.");
                        }
                    }
                    j--;
                }
                if (j < 0)
                    throw new CompilerSyntaxException("Braces syntax error.");
                PartialSignature signature = new PartialSignature();
                signature.OperationName = operation.Name;
                signature.SignaturePartNumber = partNumber;
                //
                stack.Add(new OperationsStackItem(OperationStackItemKind.PartialSignature, signature));
                //
                if (partNumber == operation.Signature.Length) {
                    for (int ii = 0; ii < partNumber; ii++) {
                        stack.RemoveAt(stack.Count - 1);
                    }
                    stack.Add(new OperationsStackItem(OperationStackItemKind.Operation, operation.Name));
                }
            }

            public void DoFinalFlush() {
                // Pop all from the stack
                for (int j = stack.Count - 1; j >= 0; j--) {
                    switch (stack[j].Kind) {
                        case OperationStackItemKind.Operation: {
                            res.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Operation, stack[j].OperationName));
                            break;
                        }
                        default: {
                            throw new CompilerSyntaxException("Syntax error. Unexpected item in stack.");
                        }
                    }
                }
            }

            public CompiledExpression GetResult() {
                return new CompiledExpression(res);
            }
        }

        private sealed class OperationsStackItem {
            private readonly OperationStackItemKind kind;
            public OperationStackItemKind Kind {
                get {
                    return kind;
                }
            }

            private readonly DelimiterKind delimiter;
            public DelimiterKind Delimiter {
                get {
                    if (kind != OperationStackItemKind.Delimiter)
                        throw new InvalidOperationException("Type mismatch.");
                    return delimiter;
                }
            }

            private readonly string operationName;
            public string OperationName {
                get {
                    if (kind != OperationStackItemKind.Operation)
                        throw new InvalidOperationException("Type mismatch.");
                    return operationName;
                }
            }

            private readonly PartialSignature partialSignature;
            public PartialSignature PartialSignature {
                get {
                    if (kind != OperationStackItemKind.PartialSignature)
                        throw new InvalidOperationException("Type mismatch.");
                    return partialSignature;
                }
            }

            public OperationsStackItem(OperationStackItemKind kind, object value) {
                if (value == null)
                    throw new ArgumentNullException("value");
                //
                this.kind = kind;
                switch (kind) {
                    case OperationStackItemKind.Delimiter: {
                        delimiter = (DelimiterKind) value;
                        break;
                    }
                    case OperationStackItemKind.Operation: {
                        operationName = (string) value;
                        break;
                    }
                    case OperationStackItemKind.PartialSignature: {
                        partialSignature = (PartialSignature) value;
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unexpected item kind.");
                    }
                }
            }
        }

        private struct PartialSignature {
            private string _OperationName;
            public string OperationName {
                get {
                    return _OperationName;
                }
                set {
                    _OperationName = value;
                }
            }

            private int _SignaturePartNumber;
            public int SignaturePartNumber {
                get {
                    return _SignaturePartNumber;
                }
                set {
                    _SignaturePartNumber = value;
                }
            }
        }

        private enum OperationStackItemKind {
            Delimiter = 1,
            Operation = 2,
            PartialSignature = 3
        }

        #endregion
    }
}