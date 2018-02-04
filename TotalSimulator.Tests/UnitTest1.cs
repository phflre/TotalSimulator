using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TotalSimulator;

namespace TotalSimulator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string data = @"101438 4585136.1816 8526588.1841 1037.2170
            100604 4583803.1520 8526260.6510 917.2440
            110001 4585131.2406 8526586.9701 1037.0200 *
            110001 4585131.2406 8526586.9701 1037.0200
            100604 4583803.1520 8526260.6510 917.2440
            110002 4585086.0137 8526476.1961 1027.6300 *
            110002 4585086.0137 8526476.1961 1027.6300
            110001 4585131.2406 8526586.9701 1037.0200
            110003 4585118.7967 8526415.4468 1024.2300
            201 4585068.5840 8526409.3740 1018.3010
            202 4585070.5210 8526394.0760 1016.1110";
        [TestMethod]
        public void TestSetData()
        {
            var simulator = new TotalSimulator();
            simulator.SetMeasurements(data);
            var resultDpi = simulator.GetDpiData();

        }
    }
}
