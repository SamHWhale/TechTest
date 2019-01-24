using Test.Models;

namespace Projections.Web.Models
{
    public class ForecastRequest
    {
        public RiskLevel RiskLevel { get; set; }
        public int TimescaleYears { get; set; }
        public decimal MonthlyInvestment { get; set; }
        public decimal LumpSumInvestment { get; set; }
        public decimal TargetValue { get; set; }

    }
}
