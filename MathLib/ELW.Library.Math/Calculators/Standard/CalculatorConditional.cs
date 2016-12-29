using System;

namespace ELW.Library.Math.Calculators.Standard {
    internal sealed class CalculatorConditional : IOperationCalculator {
        #region IOperationCalculator Members

        public double Calculate(params double[] parameters) {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (parameters.Length != 3)
                throw new ArgumentException("It is ternary operation. Parameters count should be equal to 3.", "parameters");
            //
            return (parameters[0] >= 0 ? parameters[1] : parameters[2]);
        }

        #endregion
    }
}