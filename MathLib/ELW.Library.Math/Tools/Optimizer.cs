using System;
using System.Collections.Generic;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;

namespace ELW.Library.Math.Tools {
    /// <summary>
    /// Implements simple optimization logic (constants pre-calculating algorithm).
    /// </summary>
    public sealed class Optimizer {
        private readonly OperationsRegistry operationsRegistry;
        public OperationsRegistry OperationsRegistry {
            get {
                return operationsRegistry;
            }
        }

        public Optimizer(OperationsRegistry operationsRegistry) {
            if (operationsRegistry == null)
                throw new ArgumentNullException("operationsRegistry");
            //
            this.operationsRegistry = operationsRegistry;
        }

        public CompiledExpression Optimize(CompiledExpression compiledExpression) {
            if (compiledExpression == null)
                throw new ArgumentNullException("compiledExpression");
            //
            List<CompiledExpressionItem> optimizedExpression = new List<CompiledExpressionItem>();
            //
            for (int i = 0; i < compiledExpression.CompiledExpressionItems.Count; i++) {
                CompiledExpressionItem item = compiledExpression.CompiledExpressionItems[i];
                //
                switch (item.Kind) {
                    case CompiledExpressionItemKind.Constant: {
                        optimizedExpression.Add(item);
                        break;
                    }
                    case CompiledExpressionItemKind.Variable: {
                        optimizedExpression.Add(item);
                        break;
                    }
                    case CompiledExpressionItemKind.Operation: {
                        Operation operation = operationsRegistry.GetOperationByName(item.OperationName);
                        // If all arguments are constants, we can optimize this. Otherwise, we can't
                        bool noVariablesInArguments = true;
                        for (int j = 0; (j < operation.OperandsCount) && noVariablesInArguments; j++) {
                            int index = optimizedExpression.Count - operation.OperandsCount + j;
                            if (index < 0)
                                throw new MathProcessorException("Stack is empty.");
                            //
                            if (optimizedExpression[index].Kind != CompiledExpressionItemKind.Constant)
                                noVariablesInArguments = false;
                        }
                        if (noVariablesInArguments) {
                            double[] arguments = new double[operation.OperandsCount];
                            for (int j = optimizedExpression.Count - operation.OperandsCount, k = 0; j < optimizedExpression.Count; j++, k++) {
                                arguments[k] = optimizedExpression[j].Constant;
                            }
                            //
                            optimizedExpression.RemoveRange(optimizedExpression.Count - operation.OperandsCount, operation.OperandsCount);
                            optimizedExpression.Add(new CompiledExpressionItem(CompiledExpressionItemKind.Constant,
                                                                               operation.Calculator.Calculate(arguments)));
                        } else {
                            optimizedExpression.Add(item);
                        }
                        break;
                    }
                    default: {
                        throw new InvalidOperationException("Unknown item kind.");
                    }
                }
            }
            return new CompiledExpression(optimizedExpression);
        }
    }
}