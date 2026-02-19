# NewtonRootFinder

Biblioteka C# do znajdowania wszystkich pierwiastków rzeczywistych wielomianów metodą Newtona z deflacją.


## Skąd pomysł?

Podczas zajęć z programowania gdy szybko skończyłem proste zadanie, 
nauczyciel dał mi dodatkowe trudniejsze. Moim zadaniem było zaimplementować metodę Newtona dla funkcji kwadratowej 
aby pokazać miejsca zerowe wielomianów. Gdy zacząłem szukać gotowych przykładów w C# okazało się 
że w internecie trudno znaleźć implementację która łączy metodę Newtona 
z deflacją wielomianu i jest napisana w sposób zrozumiały.

## Instalacja

```bash
dotnet add package NewtonRootFinder
```

## Użycie
```csharp
using NewtonRootFinder;

double[] coefficients = { 1, -6, 11, -6 }; // x³ - 6x² + 11x - 6
var roots = RootFinder.FindAllRoots(coefficients);

foreach (var root in roots)
{
    Console.WriteLine(root); // 0,9999999999999996 | 2,000000000000001 | 2,999999999999999
}
```

## Jak to działa?

Biblioteka używa metody Newtona połączonej z deflacją wielomianu. Algorytm działa w trzech krokach:

1. Dla każdego punktu startowego z zakresu [-10, 10] wywołuje metodę Newtona
2. Po znalezieniu pierwiastka dzieli wielomian przez (x - pierwiastek) deflacja obniża stopień wielomianu
3. Powtarza proces na wielomianie niższego stopnia aż znajdzie wszystkie pierwiastki

Do obliczania wartości wielomianu i jego pochodnej używany jest schemat Hornera, który jest szybszy i bardziej stabilny numerycznie niż liczenie potęg.

## Parametry

| Parametr | Typ | Opis | Domyślnie |
|----------|-----|------|-----------|
| coefficients | double[] | Współczynniki wielomianu od najwyższej potęgi | wymagany |
| epsilon | double | Dokładność przybliżenia | 1e-9 |

## Słownik pojęć

**Metoda Newtona** - algorytm do znajdowania pierwiastków funkcji. Zaczyna od punktu startowego i iteracyjnie poprawia przybliżenie według wzoru `x = x - f(x) / f'(x)` aż wynik jest wystarczająco bliski zeru.

**Schemat Hornera** - efektywny sposób obliczania wartości wielomianu. Zamiast liczyć potęgi osobno, składa wynik w jednym przebiegu pętli tylko mnożenie i dodawanie na każdym kroku. Jest szybszy i mniej podatny na błędy zaokrągleń niż tradycyjne podejście.

**Deflacja wielomianu** - po znalezieniu pierwiastka `r` dzielimy wielomian przez `(x - r)`, co obniża jego stopień o jeden. Dzięki temu możemy szukać kolejnych pierwiastków w prostszym wielomianie.

## Notatka

Projekt powstał z pomocą różnych asystentów AI, które pomagały mi zrozumieć algorytm metody Newtona, schemat Hornera oraz deflację wielomianu. Kod był przeze mnie sprawdzany, testowany i na każdym kroku analizowany zależało mi żeby rozumieć każdą linię, a nie tylko skopiować gotowe rozwiązanie. Przy projekcie pomagał mi również nauczyciel.

## Wkład w projekt

Jeśli znalazłeś błąd lub masz pomysł na ulepszenie śmiało otwórz [Issue](https://github.com/Kubaleek/NewtonRootFinder/issues) lub zrób forka i wyślij Pull Request. Każda pomoc mile widziana!

Jeśli biblioteka ci się przydała, zostaw gwiazdkę ⭐ to motywuje do dalszego rozwoju projektu i tworzenia kolejnych rozwiązań trudnych problemów.

## Licencja

MIT © 2025 Kuba 'Kubaleek' Król

Szczegóły licencji znajdziesz w pliku [LICENSE](LICENSE).
