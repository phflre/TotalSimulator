using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalSimulator.Models;

namespace TotalSimulator
{
    public class GeometryEngine
    {
        public static double GetDistance(Point firstPoint, Point secondPoint, bool vertical = false)
        {
            var deltaX = Math.Abs(firstPoint.North - secondPoint.North);
            var deltaY = Math.Abs(firstPoint.East - secondPoint.East);
            var horizontalDistance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            if (vertical)
            {
                return horizontalDistance;
            }
            else
            {
                return Math.Sqrt(Math.Pow(horizontalDistance, 2) + Math.Pow(Math.Abs(firstPoint.Z - secondPoint.Z), 2));
            }
        }

        public static double GetAzimuth(double deltaX, double deltaY)
        {
            var addition_angle = 0;
            var multiplic = 0;

            if (deltaX > 0 && deltaY > 0)
            {
                addition_angle = 0;
                multiplic = 1;
            }

            else if (deltaX < 0 && 0 < deltaY)
            {
                addition_angle = -200;
                multiplic = -1;
            }

            else if (deltaX < 0 && deltaY < 0)
            {
                addition_angle = 200;
                multiplic = 1;
            }
            else
            {
                addition_angle = -400;
                multiplic = -1;
            }

            return (Math.Atan(Math.Abs(deltaY) / Math.Abs(deltaX)) * 200 / Math.PI +
                               addition_angle) * multiplic;
        }

        public static double GetAzimuth(Point firstPoint, Point secondPoint)
        {
            var deltaX = secondPoint.North - firstPoint.North;
            var deltaY = secondPoint.East - firstPoint.East;
            return GeometryEngine.GetAzimuth(deltaX, deltaY);

                //measured_zenith1 = 
        }
        public static double GetZAngle(Point firstPoint, Point secondPoint)
        {
            var distance = GeometryEngine.GetDistance(firstPoint, secondPoint, true);
            return Math.Atan((firstPoint.Z - secondPoint.Z) / distance) * (200 / Math.PI) + 100;
        }
    }
}
