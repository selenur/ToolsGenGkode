using System;
using System.Collections.Generic;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;

namespace ELW.Library.Math.Tools {
    public sealed class Parser {
        private readonly OperationsRegistry _OperationsRegistry;
        public OperationsRegistry OperationsRegistry {
            get {
                return _OperationsRegistry;
            }
        }

        public Parser(OperationsRegistry operationsRegistry) {
            if (operationsRegistry == null)
                throw new ArgumentNullException("operationsRegistry");
            //
            _OperationsRegistry = operationsRegistry;
        }

        /// <summary>
        /// Prepares source string for compilation.
        /// </summary>
        public PreparedExpression Parse(string sourceString) {
            if (sourceString == null)
                throw new ArgumentNullException("sourceString");
            if (sourceString.Length == 0)
                throw new ArgumentException("String is empty.", "sourceString");
            // Signatures lenghts
            int[] lens = _OperationsRegistry.SignaturesLens;
            //
            List<PreparedExpressionItem> res = new List<PreparedExpressionItem>();
            bool operandStarted = false;
            int operandStartIndex = 0;
            //
            for (int i = 0; i < sourceString.Length; i++) {
                PreparedExpressionItem additionalItem = null;
                // Check for delimiters
                if ((sourceString[i] == '(') || (sourceString[i] == ')') || (sourceString[i] == ',')) {
                    // Storing delimiter
                    DelimiterKind delimiterKind = new DelimiterKind();
                    switch (sourceString[i]) {
                        case '(': {
                            delimiterKind = DelimiterKind.OpeningBrace;
                            break;
                        }
                        case ')': {
                            delimiterKind = DelimiterKind.ClosingBrace;
                            break;
                        }
                        case ',': {
                            delimiterKind = DelimiterKind.Comma;
                            break;
                        }
                    }
                    additionalItem = new PreparedExpressionItem(PreparedExpressionItemKind.Delimiter, delimiterKind);
                }
                // If not found, check for signatures, from max length to min
                if (additionalItem == null) {
                    for (int j = lens.Length - 1; j >= 0; j--) {
                        if (i + lens[j] <= sourceString.Length) {
                            // If signature found
                            if (_OperationsRegistry.IsSignatureDefined(sourceString.Substring(i, lens[j]))) {
                                // Storing signature
                                additionalItem = new PreparedExpressionItem(PreparedExpressionItemKind.Signature, sourceString.Substring(i, lens[j]));
                                break;
                            }
                        }
                    }
                }
                // If not found, working with operand
                if (additionalItem == null) {
                    if (!operandStarted) {
                        operandStarted = true;
                        operandStartIndex = i;
                    }
                } else {
                    // NOTE: Duplicate code
                    // Storing operand (constant or variable)
                    if (operandStarted) {
                        string operandString = sourceString.Substring(operandStartIndex, i - operandStartIndex);
                        double constant;
                        if (Double.TryParse(operandString.Replace('.', ','), out constant)) {
                            res.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Constant, constant));
                        } else {
                            if (!IsValidVariableName(operandString))
                                throw new CompilerSyntaxException(String.Format("{0} is not valid variable identifier.", operandString));
                            //
                            res.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Variable, operandString));
                        }
                        operandStarted = false;
                    }
                    // Delayed storing a delimiter or signature
                    res.Add(additionalItem);
                    // If additionalItem was a signature, we should add correct i index according to signature lenght
                    if (additionalItem.Kind == PreparedExpressionItemKind.Signature)
                        i += additionalItem.Signature.Length - 1;
                }
            }
            // Storing operand (constant or variable)
            if (operandStarted) {
                string operandString = sourceString.Substring(operandStartIndex);
                double constant;
                if (Double.TryParse(operandString.Replace('.', ','), out constant)) {
                    res.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Constant, constant));
                } else {
                    if (!IsValidVariableName(operandString))
                        throw new CompilerSyntaxException(String.Format("{0} is not valid variable identifier.", operandString));
                    //
                    res.Add(new PreparedExpressionItem(PreparedExpressionItemKind.Variable, operandString));
                }
            }
            //
            return new PreparedExpression(res);
        }

        public static bool IsValidVariableName(string @string) {
            if (@string == null)
                throw new ArgumentNullException("string");
            // Empty strings are not allowed
            if (@string.Length == 0)
                return (false);
            // Variable must be started from letter
            if (!Char.IsLetter(@string[0]))
                return (false);
            // All symbols must be letter or digit
            foreach (char c in @string)
                if (!Char.IsLetterOrDigit(c))
                    return (false);
            //
            return (true);
        }
    }
}