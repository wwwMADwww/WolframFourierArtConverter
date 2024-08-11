using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WolframFourierArtParser;


public struct Vector2d
{
    public Vector2d() { }

    public Vector2d(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X;
    public double Y;
}


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

    public Vector2d? Calculate(double t)
    {
        var tScaled = ScaleArgument(t);

        if (!(_series.IntervalStart <= tScaled && tScaled <= _series.IntervalEnd)) return null;

        // this present in all original expressions for some reason
        if (Math.Sin(tScaled / 2) < 0) return null;

        var x = _series.HarmonicsX.Sum(h => h.Amp * Math.Sin(h.Phase + h.N * tScaled)) + _series.ConstX;

        var y = _series.HarmonicsY.Sum(h => h.Amp * Math.Sin(h.Phase + h.N * tScaled)) + _series.ConstY;

        return new Vector2d(x, y);
    }
}
