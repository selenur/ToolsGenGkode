using System;

namespace ELW.Library.Math.Calculators.Standard {
    internal sealed class CalculatorNegation : IOperationCalculator {
        #region IOperationCalculator Members

        public double Calculate(params double[] parameters) {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (parameters.Length != 1)
                throw new ArgumentException("It is unary operation. Parameters count should be equal to 1.", "parameters");
            //
            return (-parameters[0]);
        }

        #endregion
    }
}