using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WolframFourierArtParser;

public partial class FourierSeriesParser
{
    [GeneratedRegex(@"(?<thetas>θ\((?<start>\d+) π - t\) ((θ\(t - (?<end>\d+) π\))|(θ\(t \+ π\))))")]
    private static partial Regex RegexThetas();

    [GeneratedRegex(@"(?<harm>(?<sign>[+-] ?)?(?<amp>((\d+\/\d+)|(\d+)) )?(?<sin>sin\((?<sinArg>([\d +-/tπ]+))\)))")]
    private static partial Regex RegexHarmonic();


    public FourierSeries[] ParseFile(string formulaFilePath)
    { 
        var formulas = File.ReadAllLines(formulaFilePath).ToArray();

        var xformula = formulas[0]["x(t) = ".Length..];
        var yformula = formulas[1]["y(t) = ".Length..];
        
        return Parse(xformula, yformula);
    }


    public FourierSeries[] Parse(string xFormula, string yFormula)
    {
        var xSeries = ConvertToSeries(xFormula);
        var ySeries = ConvertToSeries(yFormula);

        var series = xSeries
            .Zip(ySeries)
            .Select(h => new FourierSeries()
            { 
                IntervalStart = h.First.minT,
                IntervalEnd = h.First.maxT,
                HarmonicsX = h.First.harmonics,
                HarmonicsY = h.Second.harmonics,
            })
            .ToArray();

        return series;
    }

    private (Harmonic[] harmonics, double minT, double maxT)[] ConvertToSeries(string formula)
    {
        var funcs = new List<(Harmonic[], double, double)>();

        if (!formula.Contains('θ'))
        {
            var harmonics = ConvertToHarmonics(formula);
            funcs.Add((harmonics, 0, 2));
        }
        else
        {
            var matches = RegexThetas().Matches(formula);

            formula = formula["(".Length .. ^") θ(sqrt(sgn(sin(t/2))))".Length];

            var prevPos = 0;

            foreach (Match match in matches)
            {
                var maxts = match.Groups["start"].Value;
                var mints = match.Groups["end"].Value;

                var minT = mints.Length > 0 ? double.Parse(mints) : 0;
                var maxT = double.Parse(maxts);

                var seriesStr = formula[prevPos..(match.Index-2)];

                prevPos = match.Index + match.Length;

                var harmonics = ConvertToHarmonics(seriesStr);

                funcs.Add((harmonics, minT, maxT));
            }

        }

        return funcs.ToArray();
    }


    private Harmonic[] ConvertToHarmonics(string str)
    { 
        var harmonicMatches = RegexHarmonic().Matches(str);

        var list = new List<Harmonic>();

        foreach (Match match in harmonicMatches)
        {
            var signStr = match.Groups["sign"].Value;

            var sign = signStr.Length == 0 ? 1.0 :
                signStr[0] == '+' ? 1.0 : -1.0;

            var ampStr = match.Groups["amp"].Value;
            var amp = ampStr.Length == 0 ? 1.0 : CalcFraction(ampStr);

            var sinArgStr = match.Groups["sinArg"].Value;

            var (n, phase) = sinArgStr.Length == 0
                ? (0, double.NaN)
                : GetSinArgComponents(sinArgStr);

            var harmonic = new Harmonic()
            {
                Amp = amp * sign,
                N = n,
                Phase = phase,
            };

            list.Add(harmonic);
        }

        var trail = str[(harmonicMatches[^1].Index + harmonicMatches[^1].Length)..].Replace(")", "").Trim();

        if (trail.Length > 0)
        {
            var trailSign = trail[0] == '-' ? -1.0 : 1.0;

            var trailAmpStr = trail[2..];
            var trailAmp = CalcFraction(trailAmpStr);

            var trailHarmonic = new Harmonic()
            {
                Amp = trailAmp * trailSign,
                N = 0,
                Phase = 0,
            };
            list.Add(trailHarmonic);
        }

        return list.ToArray();  
    }

    (int n, double phase) GetSinArgComponents(string str)
    {
        int GetN(string str) 
        {
            var nStr = str.Replace("t", "").Trim();
            var n = nStr.Length == 0 ? 1 : int.Parse(nStr);
            return n;
        }

        if (str.Contains('+'))
        {
            var components = str.Split('+');
            var n = GetN(components[0]);
            var phase = CalcFraction(components[1]);
            return (+ n, phase);
        }
        else if (str.Contains('-'))
        {
            var components = str.Split('-');
            var phase = CalcFraction(components[0]);
            var n = GetN(components[1]);
            return (- n, phase);
        }
        else
        {
            return (GetN(str), 0.0);
        }
    }

    double CalcFraction(string s)
    {
        var args = s.Trim().Split('/');
        if (args.Length == 1) return double.Parse(args[0]);
        else return double.Parse(args[0]) / double.Parse(args[1]);
    }

}
