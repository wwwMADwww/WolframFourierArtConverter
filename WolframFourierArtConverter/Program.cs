using System.Globalization;

namespace WolframFourierArtConverter;

internal class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length <= 1)
        {
            PrintUsage();
            return;
        }

        if (args[0].ToLower() == "convert")
        {
            await ConvertCommand.Convert(args[1], args.Length > 2 ? args[2] : null);
            return;
        }

        if (args[0].ToLower() == "generate")
        {
            if (args.Length < 3)
            {
                PrintUsage();
                return;
            }

            await GenerateCommand.Generate(args[1], float.Parse(args[2], CultureInfo.InvariantCulture), args.Length > 3 ? args[3] : null);
            return;
        }

        Console.WriteLine("Done");
    }

    static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  WolframFourierArtConverter convert formulaInputFilename [outputFilename]");
        Console.WriteLine("  WolframFourierArtConverter generate formulaInputFilename argumentStep [outputFilename]");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  WolframFourierArtConverter convert formula.txt");
        Console.WriteLine("  WolframFourierArtConverter convert formula.txt harmonics.json");
        Console.WriteLine("  WolframFourierArtConverter generate formula.txt 0.0001");
        Console.WriteLine("  WolframFourierArtConverter generate harmonics.json 0.0001 coordinates.csv");
        Console.WriteLine();
    }
}
