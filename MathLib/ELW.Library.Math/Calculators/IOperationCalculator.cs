namespace ELW.Library.Math.Calculators {
    /// <summary>
    /// Common interface shoud be implemented in variuos operations calculators.
    /// </summary>
    public interface IOperationCalculator {
        /// <summary>
        /// Returns a result of operation called.
        /// </summary>
        double Calculate(params double[] parameters);
    }
}