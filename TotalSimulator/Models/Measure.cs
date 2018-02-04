using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalSimulator.Models
{
    public class Measure: IPointMeasure
    {
        public int Number { get; set; }
        public double HAngle { get; set; }
        public double ZAngle { get; set; }
        public double Distance { get; set; }
        public double SignalHeight { get; set; }
        public Measure()
        {

        }
        public Measure(Measure measure)
        {
            this.Number = measure.Number;
            this.HAngle = measure.HAngle;
            this.ZAngle = measure.ZAngle;
            this.Distance = measure.Distance;
            this.SignalHeight = measure.SignalHeight;
        }
        public void FlipHorizontaly()
        {
            this.HAngle += 200;
            if(this.HAngle >= 400)
            {
                this.HAngle -= 400;
            }
        }

        public void FlipVerticaly()
        {
            this.ZAngle = 400 - this.ZAngle;
        }
        public void FlipBothWays()
        {
            this.FlipHorizontaly();
            this.FlipVerticaly();
        }

    }
}
