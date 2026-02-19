using NewtonRootFinder;

namespace NewtonRootFinder.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void FindAllRoots_LinearPolynomial_ReturnsOneRoot()
        {
            double[] coef = { 2, -4 }; // 2x - 4 = 0 → x = 2

            List<double> roots = RootFinder.FindAllRoots(coef);

            Assert.Single(roots);
            Assert.Equal(2.0, roots[0], precision: 4);
        }
        [Fact]
        public void FindAllRoots_QuadraticPolynomial_ReturnsTwoRoots()
        {
            double[] coef = { 1, 0, -4 }; // x² - 4 = 0 → x = 2, x = -2

            List<double> roots = RootFinder.FindAllRoots(coef);

            Assert.Equal(2, roots.Count);
            Assert.Contains(roots, root => Math.Abs(root - 2.0) < 1e-4);
            Assert.Contains(roots, root => Math.Abs(root + 2.0) < 1e-4);
        }

        [Fact]
        public void FindAllRoots_CubicPolynomial_ReturnsThreeRoots()
        {
            double[] coef = { 1, -6, 11, -6 }; // x³ - 6x² + 11x - 6 → x = 1, 2, 3
            List<double> roots = RootFinder.FindAllRoots(coef);

            Assert.Equal(3, roots.Count);
            Assert.Contains(roots, root => Math.Abs(root - 1.0) < 1e-4);
            Assert.Contains(roots, root => Math.Abs(root - 2.0) < 1e-4);
            Assert.Contains(roots, root => Math.Abs(root - 3.0) < 1e-4);
        }

        [Fact]
        public void FindAllRoots_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RootFinder.FindAllRoots(null!));
        }

        [Fact]
        public void FindAllRoots_TooShortArray_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RootFinder.FindAllRoots(new double[] { 5 }));
        }

        [Fact]
        public void FindAllRoots_ZeroLeadingCoefficient_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RootFinder.FindAllRoots(new double[] { 0, 1, 2 }));
        }

        [Fact]
        public void FindAllRoots_NegativeEpsilon_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RootFinder.FindAllRoots(new double[] { 1, -1 }, -0.1));
        }

    }
}