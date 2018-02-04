using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TotalSimulator.Models;

namespace TotalSimulator
{
    public class TotalSimulator : ITotalSimulator
    {
        private Regex measureRegex = new Regex(@"^(\d+) {1}(\d+[.]{0,1}\d+) {1}(\d+[.]{0,1}\d+) {0,1}(\d+[.]{0,1}\d+)?$");
        private List<Point> allMeasuredWGFPoints = new List<Point>();
        private List<Point> allMeasuredCommonPoints = new List<Point>();

        public IReadOnlyList<Point> AllMeasuredWGFPoints { get { return this.allMeasuredWGFPoints.AsReadOnly(); } }
        public IReadOnlyList<Point> AllMeasuredCommonPoints { get { return this.allMeasuredCommonPoints.AsReadOnly(); } }
        public List<string> Errors { get; set; } = new List<string>();
        public TotalStation[] TotalStations { get; private set; }

        public string GetDpiData()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"ОБЕКТ: \n   9  5   2   2    0   5  5  0  5  30   5   5   5\nNS         I         T         R         Z         S\n");
            foreach (var station in this.TotalStations)
            {
                stringBuilder.Append(station.GetDpiData());
            }
            return stringBuilder.ToString();
        }

        public string GetKorData(IEnumerable<Point> wgfPoints)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(" О Б Е К Т : \n Номер Клас      X           Y    Клас   H         Mx     My    Rxy      Mh\n     0  0       0.000       0.000  3    0.0000    0.0    0.0   0.000    0.00\n");
            foreach (var point in wgfPoints)
            {
                stringBuilder.Append($"{point.Number}  8{point.North,14:0.###}{point.East,14:0.###}  8{point.Z,10:0.###}    0.0    0.0   0.000    0.00\n");
            }
            return stringBuilder.ToString();
        }

        public void SetMeasurements(string measuredPointsData)
        {
            var totalStations = new List<TotalStation>();
            var currentRow = 0;
            var rawStationsData = measuredPointsData.Split('*');
            foreach (var rawStationData in rawStationsData)
            {
                var rawMeasuresData = rawStationData.Split('\n').Select(x => x.Trim(new char[] { ' ', '\t', '\r' })).ToArray();
                TotalStation totalStation = null;
                var stationMeasuredPoints = new List<Point>();
                for (int i = 0; i < rawMeasuresData.Length; i++)
                {
                    currentRow++;
                    rawMeasuresData[i] = Regex.Replace(rawMeasuresData[i], @"\s+", " ");
                    rawMeasuresData[i] = Regex.Replace(rawMeasuresData[i], @"\t+", " ");
                    if (string.IsNullOrEmpty(rawMeasuresData[i]))
                    {
                        continue;
                    }
                    var point = this.ConvertDataToPoint(rawMeasuresData[i]);
                    if(point == null)
                    {
                        Errors.Add($"Грешка в ред {currentRow} \"{rawMeasuresData[i]}\"");
                        continue;
                    }
                    if (point.isWGFPoint)
                    {
                        if(!allMeasuredWGFPoints.Contains(point, new PointComparer()))
                        {
                            allMeasuredWGFPoints.Add(point);
                        }
                    }
                    else
                    {
                        if (allMeasuredCommonPoints.Select(p => p.Number).Contains(point.Number)) this.Errors.Add($"Подробна точка {point.Number} е измерена повече от веднъж");
                        allMeasuredCommonPoints.Add(point);
                    }
                    if (totalStation == null)
                    {
                        totalStation = new TotalStation(point, 1.64f, 1, 1);
                    }
                    else
                    {
                        stationMeasuredPoints.Add(point);
                    }

                }
                if(totalStation != null)
                {
                    if(stationMeasuredPoints.Count() == 0)
                    {
                        this.Errors.Add($"Няма измерени точки от станция {totalStation.opticsCenter.Number}");
                    }
                    if(this.Errors.Count() == 0)
                    {
                        totalStation.Measure(stationMeasuredPoints);
                        totalStations.Add(totalStation);
                    }
                }
            }
            this.TotalStations = totalStations.ToArray();
        }

        protected Point ConvertDataToPoint(string pointData)
        {
            var match = measureRegex.Match(pointData);
            if (match.Success)
            {
                return new Point(
                    int.Parse(match.Groups[1].Value),
                    double.Parse(match.Groups[2].Value),
                    double.Parse(match.Groups[3].Value),
                    double.Parse(match.Groups[4].Value));
            }
            return null;
        }
    }
}
