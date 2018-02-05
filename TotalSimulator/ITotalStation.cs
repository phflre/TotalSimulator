using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalSimulator.Models;

namespace TotalSimulator
{
    public interface ITotalStation
    {
        void Measure(IEnumerable<Point> points);
        Point Position { get; set; }
        void ReMeasureCommonPoints();
    }

}
