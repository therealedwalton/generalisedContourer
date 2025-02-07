using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core
{
    public class GaussianDistribution2D
    {
        public Point Centre { get; set; }

        public double Amplitude { get; set; }

        public double SigmaX { get; set; }

        public double SigmaY { get; set; }

        public GaussianDistribution2D(Point centre, double amplitude, double sigmaX, double sigmaY)
        {
            Centre = centre;
            Amplitude = amplitude;
            SigmaX = sigmaX;
            SigmaY = sigmaY;
        }

        public double Value(double x, double y)
        {
            // Calculate the x and y differences from the center
            double dx = x - Centre.x;
            double dy = y - Centre.y;

            // Calculate the exponential terms
            double exponentX = -(dx * dx) / (2 * SigmaX * SigmaX);
            double exponentY = -(dy * dy) / (2 * SigmaY * SigmaY);

            // Calculate the full 2D Gaussian equation
            // f(x,y) = A * exp(-(x-x₀)²/(2σx²) - (y-y₀)²/(2σy²))
            double gaussian = Amplitude * Math.Exp(exponentX + exponentY);

            return gaussian;
        }
    }
}
