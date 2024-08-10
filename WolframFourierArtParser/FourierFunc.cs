using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WolframFourierArtParser;

public class FourierFunc
{
    private readonly FourierSeries _series;

    public FourierFunc(FourierSeries series)
    {
        _series = series;
    }

    private double ScaleArgument(double t)
    {
        var tt = _series.IntervalStart + t * (_series.IntervalEnd - _series.IntervalStart);
        return tt;
    }

    public Vector2? Calculate(double t)
    {
        t = ScaleArgument(t);

        if (!(_series.IntervalStart <= t && t <= _series.IntervalEnd)) return null;

        // this present in all original expressions for some reason
        if (Math.Sin((t * Math.PI) / 2) < 0) return null;

        var x = _series.HarmonicsX.Sum(h => h.N != 0 
            ? h.Amp * Math.Sin(h.Phase + h.N * t * Math.PI)
            : h.Amp);

        var y = _series.HarmonicsY.Sum(h => h.N != 0
            ? h.Amp * Math.Sin(h.Phase + h.N * t * Math.PI)
            : h.Amp);

        return new Vector2((float) x, (float) y);
    }
}
