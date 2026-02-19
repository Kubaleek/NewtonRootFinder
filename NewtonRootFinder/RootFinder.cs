namespace NewtonRootFinder
{
    public static class RootFinder
    {
        /// <summary>
        /// Znajduje wszystkie pierwiastki rzeczywiste wielomianu za pomocą metody Newtona.
        /// Funkcja wywołuje NewtonMethod z różnych punktów startowych, a po każdym znalezionym pierwiastku
        /// wykonuje deflację wielomianu, aby szukać kolejnych pierwiastków.
        /// </summary>
        /// <param name="coefficients">Tablica współczynników wielomianu od najwyższej potęgi do stałej.
        /// Przykład: {2, -6, 2, -1} oznacza 2x³ - 6x² + 2x - 1.</param>
        /// <param name="epsilon">Dokładność przybliżenia pierwiastków. Domyślnie 1e-9.</param>
        /// <returns>Lista wszystkich znalezionych pierwiastków rzeczywistych.</returns>
        /// <remarks>
        /// <para>Algorytm:</para>
        /// <list type="number">
        /// <item><description>Tworzy kopię tablicy współczynników do obliczeń.</description></item>
        /// <item><description>Dla każdego punktu startowego wywołuje NewtonMethod.</description></item>
        /// <item><description>Jeśli wynik jest pierwiastkiem i nie był wcześniej zapisany, dodaje do listy roots.</description></item>
        /// <item><description>Wykonuje deflację wielomianu, aby szukać kolejnych pierwiastków w mniejszym wielomianie.</description></item>
        /// <item><description>Jeśli pozostał wielomian liniowy, oblicza pierwiastek bez Newtona.</description></item>
        /// </list>
        /// <para>Wyjątki w NewtonMethod są łapane i ignorowane, aby kolejny punkt startowy mógł próbować znaleźć pierwiastek.</para>
        /// <para>Przykład użycia:</para>
        /// <code>
        /// double[] coef = {1, -6, 11, -6}; // x³ - 6x² + 11x - 6
        /// List&lt;double&gt; roots = FindAllRoots(coef);
        /// // wynik: [1, 2, 3]
        /// </code>
        /// </remarks>
        public static List<double> FindAllRoots(double[] coefficients, double epsilon = 1e-9)
        {
            if (coefficients == null)
                throw new ArgumentNullException(nameof(coefficients), "Tablica współczynników nie może być null.");

            if (coefficients.Length < 2)
                throw new ArgumentException("Wielomian musi mieć co najmniej dwa współczynniki (ax + b).", nameof(coefficients));

            if (coefficients[0] == 0)
                throw new ArgumentException("Współczynnik przy najwyższej potędze nie może być zerem.", nameof(coefficients));

            if (epsilon <= 0)
                throw new ArgumentOutOfRangeException(nameof(epsilon), "Epsilon musi być wartością dodatnią.");

            var roots = new List<double>();
            double[] current = (double[])coefficients.Clone();
            double[] startPoints = { -10, -3, -1, 0, 0.5, 1, 2, 5, 10 };

            while (current.Length > 2)
            {
                double? found = null;

                foreach (double start in startPoints)
                {
                    try
                    {
                        double candidate = NewtonMethod(current, start, epsilon);

                        if (Math.Abs(ValuePolynomial(current, candidate)) < epsilon &&
                            !roots.Any(root => Math.Abs(root - candidate) < 1e-4))
                        {
                            found = candidate;
                            break;
                        }
                    }
                    catch
                    {
                        // Newton nie zbiegł się dla tego punktu startowego — próbujemy następny
                    }
                }

                if (found == null)
                {
                    break;
                }

                roots.Add(found.Value);
                current = Deflate(current, found.Value);
            }

            // jeśli pozostał wielomian liniowy ax + b = 0
            if (current.Length == 2)
            {
                double linearRoot = -current[1] / current[0];
                if (!roots.Any(root => Math.Abs(root - linearRoot) < 1e-4))
                    roots.Add(linearRoot);
            }

            return roots;
        }


        /// <summary>
        /// Szuka pierwiastka wielomianu metodą Newtona od podanego punktu startowego.
        /// Metoda iteracyjnie poprawia przybliżenie pierwiastka, aż f(x) będzie bliskie zeru.
        /// </summary>
        /// <param name="a">Tablica współczynników wielomianu, od najwyższej potęgi do stałej.
        /// Przykład: {2, -6, 2, -1} oznacza 2x³ - 6x² + 2x - 1.</param>
        /// <param name="x0">Punkt startowy dla algorytmu Newtona.</param>
        /// <param name="epsilon">Dokładność przybliżenia pierwiastka. Domyślnie 1e-10.</param>
        /// <param name="maxIter">Maksymalna liczba iteracji. Domyślnie 1000.</param>
        /// <returns>Przybliżona wartość pierwiastka wielomianu.</returns>
        /// <exception cref="Exception">Rzucane, gdy pochodna wynosi 0 lub nie osiągnięto zbieżności.</exception>
        /// <remarks>
        /// <para><b>Metoda Newtona</b> służy do znajdowania pierwiastków funkcji f(x), czyli miejsc, gdzie f(x) = 0.</para>
        ///
        /// <para>Algorytm działa iteracyjnie: dla przybliżenia x_n oblicza nowe przybliżenie:</para>
        /// <code>
        /// x_{n+1} = x_n - f(x_n) / f'(x_n)
        /// </code>
        ///
        /// <para>Gdzie:</para>
        /// <list type="bullet">
        /// <item><description>f(x_n) – wartość wielomianu w punkcie x_n (ValuePolynomial)</description></item>
        /// <item><description>f'(x_n) – wartość pochodnej w punkcie x_n (DerivativePolynomial)</description></item>
        /// </list>
        ///
        /// <para>Pętla kończy się gdy:</para>
        /// <list type="bullet">
        /// <item><description>|f(x)| &lt; epsilon – osiągnięto wymaganą dokładność</description></item>
        /// <item><description>f'(x) = 0 – metoda nie może kontynuować, rzucany wyjątek</description></item>
        /// <item><description>osiągnięto maksymalną liczbę iteracji – rzucany wyjątek</description></item>
        /// </list>
        ///
        /// <para>Przykład użycia:</para>
        /// <code>
        /// double[] coef = {2, -6, 2, -1};
        /// double root = NewtonMethod(coef, 1.0);
        /// Console.WriteLine(root); // wynik ≈ 1
        /// </code>
        ///
        /// <para>Dzięki temu schematowi Newton szybko i stabilnie znajduje pierwiastki
        /// wielomianu, wykorzystując zarówno wartość funkcji, jak i jej pochodną.</para>
        /// </remarks>
        private static double NewtonMethod(double[] a, double x0, double epsilon = 1e-10, int maxIter = 1000)
        {
            double x = x0;

            for (int i = 0; i < maxIter; i++)
            {
                double fx = ValuePolynomial(a, x);

                if (Math.Abs(fx) < epsilon) return x;

                double f1x = DerivativePolynomial(a, x);

                if (f1x == 0) throw new Exception("Pochodna wynosi 0 – nie można kontynuować.");

                x -= fx / f1x;
            }

            throw new Exception("Nie osiągnięto zbieżności w maksymalnej liczbie iteracji.");
        }


        /// <summary>
        /// Oblicza wartość wielomianu w punkcie x przy użyciu schematu Hornera.
        /// Funkcja pozwala szybko i efektywnie policzyć wartość wielomianu bez liczenia potęg.
        /// </summary>
        /// <param name="a">Tablica współczynników wielomianu, od najwyższej potęgi do stałej.
        /// Przykład: {2, -6, 2, -1} oznacza 2x³ - 6x² + 2x - 1.</param>
        /// <param name="x">Punkt, w którym obliczana jest wartość wielomianu.</param>
        /// <returns>Wartość wielomianu w punkcie x.</returns>
        /// <remarks>
        /// <para><b>Wielomian</b> f(x) = a0*xⁿ + a1*xⁿ⁻¹ + ... + an jest obliczany za pomocą schematu Hornera:</para>
        /// <para>f(x) = (...((a0*x + a1)*x + a2)*x + ... + an)</para>
        ///
        /// <para><b>Schemat Hornera</b> pozwala uniknąć liczenia potęg i wielokrotnego mnożenia, 
        /// co zwiększa szybkość i stabilność numeryczną.</para>
        ///
        /// <para>Przykład działania:</para>
        /// <code>
        /// a = {2, -6, 2, -1} // 2x³ - 6x² + 2x - 1
        /// x = 1
        /// result = 0
        /// Iteracja 0: result = 0*1 + 2 = 2
        /// Iteracja 1: result = 2*1 + (-6) = -4
        /// Iteracja 2: result = -4*1 + 2 = -2
        /// Iteracja 3: result = -2*1 + (-1) = -3
        /// Wynik: f(1) = -3
        /// </code>
        ///
        /// <para>Dzięki temu schematowi wartość wielomianu w dowolnym punkcie można policzyć
        /// w jednym przebiegu pętli, szybko i stabilnie.</para>
        /// </remarks>
        private static double ValuePolynomial(double[] a, double x)
        {
            double result = 0;

            for (int i = 0; i < a.Length; i++)
            {
                result = result * x + a[i];
            }

            return result;
        }

        /// <summary>
        /// Oblicza wartość pochodnej wielomianu w punkcie x przy użyciu schematu Hornera.
        /// Funkcja pozwala szybko policzyć pochodną wielomianu bez użycia potęg i dodatkowych pętli.
        /// </summary>
        /// <param name="a">Tablica współczynników wielomianu, od najwyższej potęgi do stałej.
        /// Przykład: {2, -6, 2, -1} oznacza 2x³ - 6x² + 2x - 1.</param>
        /// <param name="x">Punkt, w którym obliczana jest wartość pochodnej.</param>
        /// <returns>Wartość pochodnej wielomianu w punkcie x.</returns>
        /// <remarks>
        /// <para><b>Pochodna wielomianu</b> f(x) = a0*xⁿ + a1*xⁿ⁻¹ + ... + an jest definiowana jako
        /// f'(x) = n*a0*xⁿ⁻¹ + (n-1)*a1*xⁿ⁻² + ... + 0*an.</para>
        ///
        /// <para><b>Schemat Hornera</b> umożliwia efektywne obliczanie pochodnej w jednym przebiegu pętli.
        /// Każdy wyraz jest składany z poprzednim przez operację result = result * x + a[i] * (stopień wyrazu),
        /// co pozwala uniknąć liczenia potęg i zmniejsza błąd numeryczny.</para>
        ///
        /// <para>Przykład działania:</para>
        /// <code>
        /// a = {2, -6, 2, -1} // 2x³ - 6x² + 2x - 1
        /// x = 1
        /// result = 0
        /// Iteracja 0: result = 0*1 + 2*3 = 6
        /// Iteracja 1: result = 6*1 + (-6)*2 = 6
        /// Iteracja 2: result = 6*1 + 2*1 = 8
        /// Iteracja 3: result = 8*1 + (-1)*0 = 8
        /// Wynik: f'(1) = 8
        /// </code>
        ///
        /// <para>Dzięki temu schematowi obliczamy pochodną szybko i numerycznie stabilnie,
        /// co jest przydatne przy metodzie Newtona do szukania pierwiastków wielomianu.</para>
        /// </remarks>
        private static double DerivativePolynomial(double[] a, double x)
        {
            double result = 0;

            for (int i = 0; i < a.Length - 1; i++)
            {
                result = result * x + a[i] * (a.Length - 1 - i);
            }

            return result;
        }


        /// <summary>
        /// Wykonuje deflację wielomianu, dzieląc go przez (x - root) przy użyciu schematu Hornera.
        /// Funkcja obniża stopień wielomianu o jeden i zwraca nową tablicę współczynników
        /// </summary>
        /// <param name="a">Tablica współczynników wielomianu, od najwyższej potęgi do stałej.
        /// Przykład {2, -6, 2, -1} oznacza 2x³ - 6x² + 2x - 1.</param>
        /// <param name="root">Znaleziony pierwiastek, przez który dzielimy wielomian (x - root).</param>
        /// <returns>Nowa tablica współczynników wielomianu po deflacji (o stopień niższa).</returns>
        /// <remarks>
        /// <para><b>Deflacja wielomianu</b> to proces dzielenia wielomianu przez (x - root), gdzie root
        /// jest znanym pierwiastkiem. Po deflacji otrzymujemy nowy wielomian stopnia niższego,
        /// w którym można szukać kolejnych pierwiastków.</para>
        ///
        /// <para><b>Schemat Hornera</b> to efektywny sposób obliczania wartości wielomianu i jednoczesnego
        /// wykonywania dzielenia przez (x - root). Pozwala uniknąć liczenia potęg i zmniejsza błąd numeryczny.
        /// W pętli funkcji kolejne współczynniki są „składane” z poprzednimi, tworząc nowy wielomian
        /// po deflacji.</para>
        ///
        /// <para>Przykład działania:</para>
        /// <code>
        /// f(x) = 2x³ - 6x² + 2x - 1, root = 1
        /// result[0] = 2
        /// result[1] = -6 + 2*1 = -4
        /// result[2] = 2 + (-4)*1 = -2
        /// Nowy wielomian po deflacji: 2x² - 4x - 2
        /// </code>
        /// </remarks>
        private static double[] Deflate(double[] a, double root)
        {
            double[] result = new double[a.Length - 1];
            double b = a[0];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = b;
                b = a[i + 1] + b * root;
            }

            return result;
        }
    }
}