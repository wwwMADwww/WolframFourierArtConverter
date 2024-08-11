using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WolframFourierArtParser;

public class FourierSeries
{
    public double IntervalStart { get; init; }

    public double IntervalEnd { get; init; }

    public Harmonic[] HarmonicsX { get; init; }

    public double ConstX { get; init; }

    public Harmonic[] HarmonicsY { get; init; }

    public double ConstY { get; init; }
}

public class Harmonic
{
    public double Amp { get; init; }
    public double Phase { get; init; }
    public int N { get; init; }
}
