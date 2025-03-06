using System.Globalization;
using Humanizer;
using Humanizer.Inflections;
using Libraries.Shared;
partial class Program
{
    private static void ConfigureConsole(string culture = "en-US")
    {
        OutputEncoding = System.Text.Encoding.UTF8;
        Thread t = Thread.CurrentThread;
        t.CurrentCulture = CultureInfo.GetCultureInfo(culture);
        t.CurrentUICulture = t.CurrentCulture;
        WriteLine("Current culture: {0}", t.CurrentCulture.DisplayName);
        WriteLine("");
    }
    private static void OutputCasings(string original)
    {
        WriteLine("Original casing: {0}", original);
        WriteLine("Lower casing: {0}", original.Transform(To.LowerCase));
        WriteLine("Upper casing: {0}", original.Transform(To.UpperCase));
        WriteLine("Title casing: {0}", original.Transform(To.TitleCase));
        WriteLine("Sentence casing: {0}", original.Transform(To.SentenceCase));
        WriteLine("Lower, then Sentence casing: {0}",
          original.Transform(To.LowerCase, To.SentenceCase));
        WriteLine();
    }

    private static void OutputSpacingAndDashes()
    {
        string ugly = "ERROR_MESSAGE_FROM_SERVICE";

        WriteLine("Original string: {0}", ugly);

        WriteLine("Humanized: {0}", ugly.Humanize());

        // LetterCasing is legacy and will be removed in future.
        WriteLine("Humanized, lower case: {0}",
          ugly.Humanize(LetterCasing.LowerCase));

        // Use Transform for casing instead.
        WriteLine("Transformed (lower case, then sentence case): {0}",
          ugly.Transform(To.LowerCase, To.SentenceCase));

        WriteLine("Humanized, Transformed (lower case, then sentence case): {0}",
          ugly.Humanize().Transform(To.LowerCase, To.SentenceCase));
    }

    private static void OutputEnumNames()
    {
        var favoriteAncientWonder = WondersOfTheAncientWorld.MAUSOLEUM_AT_HALICARNASSUS;

        WriteLine("Raw enum value name: {0}", favoriteAncientWonder);

        WriteLine("Humanized: {0}.", favoriteAncientWonder.Humanize());

        WriteLine("Humanized, then Titleized: {0}",
          favoriteAncientWonder.Humanize().Titleize());

        WriteLine("Truncated to 8 characters: {0}",
          favoriteAncientWonder.ToString().Truncate(length: 8));

        WriteLine("Kebaberized: {0}",
          favoriteAncientWonder.ToString().Kebaberize());
    }

    private static void NumberFormatting()
    {
        Vocabularies.Default.AddIrregular("biceps", "bicepsuri");
        Vocabularies.Default.AddIrregular("procuror sef", "procurori sefi");

        int number = 123;

        WriteLine($"Original number: {number}");
        WriteLine($"Roman: {number.ToRoman()}");
        WriteLine($"Words: {number.ToWords()}");
        WriteLine($"Ordinal words: {number.ToOrdinalWords()}");
        WriteLine();

        string[] things = { "vulpe", "persoana", "oaie",
      "mar", "gasca", "oaza", "cartof", "die", "liliputan",
      "procuror sef","biceps"};

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 0; j < things.Length; j++)
            {
                Write(things[j].ToQuantity(i, ShowQuantityAs.Words));

                if (j < things.Length - 1) Write(", ");
            }
            WriteLine();
        }
        WriteLine();

        int thousands = 12345;
        int millions = 123456789;

        WriteLine("Original: {0}, Metric: About {1}", thousands,
          thousands.ToMetric(decimals: 0));

        WriteLine("Original: {0}, Metric: About {1}", thousands,
          thousands.ToMetric(MetricNumeralFormats.WithSpace
            | MetricNumeralFormats.UseShortScaleWord,
            decimals: 0));

        WriteLine("Original: {0}, Metric: {1}", millions,
          millions.ToMetric(decimals: 1));
    }
}
