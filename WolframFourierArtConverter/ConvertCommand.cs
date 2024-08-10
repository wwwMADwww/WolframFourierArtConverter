using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WolframFourierArtParser;

namespace WolframFourierArtConverter;

public static class ConvertCommand
{

    public static async Task Convert(string fileInputPath, string? fileOutputPath)
    {
        fileInputPath = Path.GetFullPath(fileInputPath);

        fileOutputPath = fileOutputPath != null
            ? Path.GetFullPath(fileOutputPath)
            : Path.GetFullPath(Path.GetFileNameWithoutExtension(fileInputPath) + ".json");

        var series = new FourierSeriesParser().ParseFile(fileInputPath);

        var seriesJson = JsonSerializer.Serialize(series);
        await File.WriteAllTextAsync(fileOutputPath, seriesJson);

        Console.WriteLine("Converted");
    }

}
