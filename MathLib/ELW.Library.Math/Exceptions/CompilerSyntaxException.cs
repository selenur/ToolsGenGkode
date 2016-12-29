using System;
using System.Runtime.Serialization;

namespace ELW.Library.Math.Exceptions {
    /// <summary>
    /// Exception caused by error during compilation.
    /// </summary>
    public class CompilerSyntaxException : MathProcessorException {
        public CompilerSyntaxException() {
        }

        public CompilerSyntaxException(string message) : base(message) {
        }

        public CompilerSyntaxException(string message, Exception innerException) : base(message, innerException) {
        }

        protected CompilerSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
