using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Projections.Core;
using Projections.Core.Calculations;
using Projections.Core.Models;
using Test.Models;
using Xunit;

namespace Projections.Tests
{
    public class ForecastTests
    {

        class FakeGrowthRates : IRiskLevelAnnualGrowth
        {
            private readonly double _growthRatePercentage;

            public FakeGrowthRates(double growthRatePercentage)
            {
                _growthRatePercentage = growthRatePercentage;
            }

            public RiskLevel RiskLevel => RiskLevel.High;
            public AnnualGrowthFigures AnnualGrowthFigures  =>
                new AnnualGrowthFigures(new BoundedValue<double>(_growthRatePercentage, _growthRatePercentage), new BoundedValue<double>(_growthRatePercentage, _growthRatePercentage));
        }

        [Fact]
        public void TestGrowthRatesMonthlyConversion()
        {
            var fakeRates = new FakeGrowthRates(16.0);

            var subject = new ForecastInvestments(new IRiskLevelAnnualGrowth[] { fakeRates });

            var result = subject.ForecastInvestment(10, 0, 12, 200, fakeRates.RiskLevel);

            var expectedFirstMonth = (10.0m) * (decimal)1.0124;

            var difference = Math.Abs(expectedFirstMonth - result.DataPoints[1].NarrowBandValue.Lower);

            Assert.True(difference < 0.01m);

        }
    }
}
