using System;
using System.Runtime.Serialization;

namespace ELW.Library.Math.Exceptions {
    /// <summary>
    /// Super class for all library specific exceptions.
    /// </summary>
    public class MathProcessorException : Exception {
        public MathProcessorException() {
        }

        public MathProcessorException(string message) : base(message) {
        }

        public MathProcessorException(string message, Exception innerException) : base(message, innerException) {
        }

        protected MathProcessorException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
