using System;
using System.Collections.Generic;

namespace Projections.Core.Models
{
    public class ForecastResult
    {
        public ForecastResult()
        {
            DataPoints = new List<ForecastPoint>();
        }

        public List<ForecastPoint> DataPoints { get; set; }
    }

    public struct ForecastPoint
    {
        public DateTime Date { get; }
        public decimal TotalInvested { get; }
        public BoundedValue<decimal> NarrowBandValue { get; }
        public BoundedValue<decimal> WideBandValue { get; }
        public decimal TargetValue { get; }

        public ForecastPoint(
            DateTime date,
            decimal totalInvested,
            BoundedValue<decimal> narrowBandValue,
            BoundedValue<decimal> wideBandValue,
            decimal targetValue)
        {
            Date = date;
            TotalInvested = totalInvested;
            NarrowBandValue = narrowBandValue;
            WideBandValue = wideBandValue;
            TargetValue = targetValue;
        }
    }
}