using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Projections.Core.Models;
using Test.Models;

namespace Projections.Core.Calculations
{
    public interface IForecastInvestments
    {
        ForecastResult ForecastInvestment(decimal lumpSum, decimal monthlyInvestment, int timeInMonths, decimal targetValue, RiskLevel riskLevel);
    }

    public class ForecastInvestments : IForecastInvestments
    {
        private readonly IRiskLevelAnnualGrowth[] _growths;

        public ForecastInvestments(IEnumerable<IRiskLevelAnnualGrowth> growths)
        {
            _growths = growths.ToArray();
        }

        private AnnualGrowthFigures GetAnnualGrowthFigures(RiskLevel riskLevel)
        {
            var annualGrowth = _growths.SingleOrDefault(x => x.RiskLevel == riskLevel);
            if (annualGrowth == null) throw new InvalidOperationException($"No annual growth figures for risk level {riskLevel}");
            return annualGrowth.AnnualGrowthFigures;
        }

        public ForecastResult ForecastInvestment(decimal lumpSum, decimal monthlyInvestment, int timeInMonths, decimal targetValue, RiskLevel riskLevel)
        {
            var annualGrowth = GetAnnualGrowthFigures(riskLevel);
            var result = new ForecastResult();
            var time = DateTime.Today;

            var narrowBounds = new BoundedValue<decimal>(lumpSum, lumpSum);
            var wideBounds = new BoundedValue<decimal>(lumpSum, lumpSum);

            for (var i = 0; i < timeInMonths; i++)
            {
                var currentInvestment = lumpSum + (i * monthlyInvestment);

                var point = new ForecastPoint(
                    time.AddMonths(i),
                    currentInvestment,
                    narrowBounds,
                    wideBounds,
                    targetValue);

                narrowBounds = CalculateGrowth(narrowBounds, annualGrowth.NarrowBoundsPercentage, monthlyInvestment);
                wideBounds = CalculateGrowth(wideBounds, annualGrowth.WideBoundsPercentage, monthlyInvestment);

                result.DataPoints.Add(point);
            }

            return result;
        }

        private BoundedValue<decimal> CalculateGrowth(BoundedValue<decimal> currentValue, BoundedValue<double> growthPercentages, decimal monthlyInvestment)
        {
            var monthlyGrowthLowerPercentage = (decimal)AdjustAnnualGrowthToMonthly(growthPercentages.Lower);
            var monthlyGrowthLower = (1.0m + monthlyGrowthLowerPercentage / 100.0m) * (currentValue.Lower + monthlyInvestment);


            var monthlyGrowthUpperPercentage = (decimal)AdjustAnnualGrowthToMonthly(growthPercentages.Upper);
            var monthlyGrowthUpper = (1.0m + monthlyGrowthUpperPercentage / 100.0m) * (currentValue.Upper + monthlyInvestment);

            var result = new BoundedValue<decimal>(monthlyGrowthLower, monthlyGrowthUpper);
            return result;
        }

        private static double AdjustAnnualGrowthToMonthly(double annualGrowthPercentage) =>
            (Math.Pow(1.0 + annualGrowthPercentage / 100.0, (1.0 / 12.0)) - 1) * 100.0;
    }
}
