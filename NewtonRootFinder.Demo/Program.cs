using NewtonRootFinder;

namespace NewtonRootFinder.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Wielomian liniowy: 2x - 4 = 0 → x = 2
            Test("Liniowy", new double[] { 2, -4 });

            // Wielomian kwadratowy: x² - 5x + 6 = 0 → x = 2, x = 3
            Test("Kwadratowy", new double[] { 1, -5, 6 });

            // Wielomian sześcienny: x³ - 6x² + 11x - 6 = 0 → x = 1, 2, 3
            Test("Sześcienny", new double[] { 1, -6, 11, -6 });

            // Wielomian z pierwiastkami ujemnymi: x² + 5x + 6 = 0 → x = -2, x = -3
            Test("Ujemne pierwiastki", new double[] { 1, 5, 6 });

            // Wielomian z pierwiastkami dodatnimi i ujemnymi: x² - 4 = 0 → x = 2, x = -2
            Test("Dodatni i ujemny", new double[] { 1, 0, -4 });

            // Wielomian z jednym pierwiastkiem (podwójnym): x² - 2x + 1 = 0 → x = 1
            Test("Podwójny pierwiastek", new double[] { 1, -2, 1 });

            // Wielomian bez pierwiastków rzeczywistych: x² + 1 = 0
            Test("Brak pierwiastków", new double[] { 1, 0, 1 });

            // Wielomian czwartego stopnia: x⁴ - 10x² + 9 = 0 → x = 1, -1, 3, -3
            Test("Czwarty stopień", new double[] { 1, 0, -10, 0, 9 });
        }

        static void Test(string nazwa, double[] coef)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {nazwa} ---");
            Console.ResetColor();
            List<double> roots = RootFinder.FindAllRoots(coef);

            if (roots.Count == 0)
                Console.WriteLine("  Brak pierwiastków rzeczywistych");
            else
                foreach (double root in roots)
                    Console.WriteLine($"  Pierwiastek: {root:F6}");

            Console.WriteLine();
        }
    }
}