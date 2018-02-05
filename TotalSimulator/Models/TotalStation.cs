﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalSimulator.Models
{
    public class TotalStation: ITotalStation
    {
        public List<Point> measuredPoints = new List<Point>();
        public List<Measure> measurements = new List<Measure>();

        public double signalHeight = 1.3;
        public Point Position { get; set; } = new Point();
        public double stationHeight;
        public float maxAngleError;
        public float maxDistError;
        public const int padding = 10;

        public Point opticsCenter {
            get
            {
                return new Point(this.Position.Number, this.Position.North, this.Position.East, this.Position.Z + this.stationHeight);
            }
        }
        public TotalStation(Point position, double stationHeight, float maxAngleError, float maxDistError)
        {
            this.Position = position;
            this.stationHeight = stationHeight;
            this.maxAngleError = maxAngleError;
            this.maxDistError = maxDistError;
        }
        public TotalStation(): this(new Point(), 0,0,0)
        {

        }

        public void Measure(IEnumerable<Point> points)
        {
            this.measuredPoints.AddRange(points);
            Measure firstMeasure;
            Measure secondMeasure;
            foreach (var point in points)
            {
                var p = new Point(point.Number, point.North, point.East, point.Z + signalHeight);
                firstMeasure = new Measure()
                {
                    Number = p.Number,
                    HAngle = GeometryEngine.GetAzimuth(this.opticsCenter, p),
                    Distance = GeometryEngine.GetDistance(this.opticsCenter, p),
                    ZAngle = GeometryEngine.GetZAngle(this.opticsCenter, p),
                    SignalHeight = this.signalHeight
                };
                this.measurements.Add(firstMeasure);
                if (p.isWGFPoint)
                {
                    secondMeasure = new Measure(firstMeasure);
                    secondMeasure.FlipBothWays();
                    this.measurements.Add(secondMeasure);
                }
            }
        }

        protected void SetRandomErrors()
        {
            //TODO - implement later
        }

        public void ReMeasureCommonPoints()
        {
            var commonPoints = this.measuredPoints.Where(p => !p.isWGFPoint);
            var commonPointNumbers = commonPoints.Select(p => p.Number);
            var commonMeasures = this.measurements.Where(m => commonPointNumbers.Contains(m.Number));
            foreach (var measure in commonMeasures)
            {
                this.measurements.Remove(measure);
            }
            this.Measure(commonPoints);
        }
        
        public string GetDpiData()
        {
            
            var stringBuilder = new StringBuilder();
            var stationString = $"{this.Position.Number} {this.stationHeight,padding:0.###}";
            var stationStringLen = 0;
            stringBuilder.Append(stationString);
            var lenth = this.measurements.Count();
            for (int i = 0; i < lenth; i++)
            {
                stringBuilder.Append(new string(' ', stationStringLen) + $"{this.measurements[i].Number,padding}{this.measurements[i].SignalHeight,5:0.###}{this.measurements[i].HAngle,padding:0.####}" + 
                    $"{this.measurements[i].ZAngle,padding:0.####} {this.measurements[i].Distance,padding:0.###}{(i + 1 == lenth ? " *" : "")}{Environment.NewLine}");
                stationStringLen = stationString.Length;
            }
            return stringBuilder.ToString();
        }
    }
}
