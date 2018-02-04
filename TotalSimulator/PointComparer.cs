using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalSimulator.Models;

namespace TotalSimulator
{
    public class PointComparer : IEqualityComparer<Point>
    { 
        public bool Equals(Point x, Point y)
        {
            return x.Number == y.Number;
        }

        public int GetHashCode(Point obj)
        {
            return obj.GetHashCode();
        }
    }
}
