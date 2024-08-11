using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using WolframFourierArtParser;

namespace WolframFourierArtConverter;

public static class GenerateCommand
{

    public static async Task Generate(string fileInputPath, float argumentStep, string? fileOutputPath)
    {
        fileInputPath = Path.GetFullPath(fileInputPath);

        fileOutputPath = fileOutputPath != null
            ? Path.GetFullPath(fileOutputPath)
            : Path.GetFullPath(Path.GetFileNameWithoutExtension(fileInputPath) + " coords.csv");

        FourierSeries[] series;

        if (Path.GetExtension(fileInputPath)?.ToLower() == ".json")
        {
            var json = await File.ReadAllTextAsync(fileInputPath);
            series = JsonSerializer.Deserialize<FourierSeries[]>(json);
        }
        else
        {
            series = new FourierSeriesParser().ParseFile(fileInputPath);
        }

        var coords = GenerateCoords(series, argumentStep);

        using (var fs = new FileStream(fileOutputPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fs.SetLength(0);
            using var sw = new StreamWriter(fs);

            foreach (var v in coords)
            {
                //await sw.WriteAsync($"{v.X.ToString(CultureInfo.InvariantCulture)}\t{v.Y.ToString(CultureInfo.InvariantCulture)}\n");
                await sw.WriteAsync($"{v.X}\t{v.Y}\n");
            }
        }

        Console.WriteLine("Generated");
    }

    public static Vector2d[] GenerateCoords(FourierSeries[] fourierSeries, double step)
    {
        var list = new List<Vector2d>((int) (1.0f / step));

        var funcs = fourierSeries.Select(s => new FourierFunc(s)).ToArray();

        foreach (var func in funcs)
        {
            for (double t = 0.0; t <= 1.0; t += step)
            {
                var v = func.Calculate(t);

                if (v == null || double.IsNaN(v.Value.X) || double.IsNaN(v.Value.Y)) continue;

                list.Add(v.Value);
            }
        }

        return list.ToArray();
    }

}
