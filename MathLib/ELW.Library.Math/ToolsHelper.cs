using ELW.Library.Math.Tools;

namespace ELW.Library.Math {
    /// <summary>
    /// Quick access to tool wanted.
    /// </summary>
    public static class ToolsHelper {
        private static readonly OperationsRegistry operationsRegistry;
        public static OperationsRegistry OperationsRegistry {
            get {
                return operationsRegistry;
            }
        }
        
        private static readonly Parser parser;
        public static Parser Parser {
            get {
                return parser;
            }
        }

        private static readonly Compiler compiler;
        public static Compiler Compiler {
            get {
                return compiler;
            }
        }

        private static readonly Calculator calculator;
        public static Calculator Calculator {
            get {
                return calculator;
            }
        }

        private static readonly Optimizer optimizer;
        public static Optimizer Optimizer {
            get {
                return optimizer;
            }
        }

        private static readonly Decompiler decompiler;
        public static Decompiler Decompiler {
            get {
                return decompiler;
            }
        }

        static ToolsHelper() {
            operationsRegistry = new OperationsRegistry();
            //
            parser = new Parser(operationsRegistry);
            compiler = new Compiler(operationsRegistry);
            calculator = new Calculator(operationsRegistry);
            optimizer = new Optimizer(operationsRegistry);
            decompiler = new Decompiler(operationsRegistry);
        }
    }
}
