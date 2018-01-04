using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class Position
    {
        public double X { get; }

        public double Y { get; }

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X:0.00}, {Y:0.00})";
        }
    }
}
