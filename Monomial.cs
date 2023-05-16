namespace Polynomial
{
    internal class Monomial
    {
        public Monomial(double degree, double coefficient)
        {
            Degree = degree;
            Coefficient = coefficient;
        }
        public double Degree { get; set; }
        public double Coefficient { get; set; }
    }
}