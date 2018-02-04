using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalSimulator.Models
{
    public class Point: IPointMeasure
    {
        public int Number { get; set; }
        public double North { get; set; }
        public double East { get; set; }
        public double Z { get; set; }
        public Point()
        {

        }
        public bool isWGFPoint { get { return this.Number >= 100000; } }
        public Point(int number, double northing, double easting, double z)
        {
            this.Number = number;
            this.North = northing;
            this.East = easting;
            this.Z = z;
        }
    }
}
