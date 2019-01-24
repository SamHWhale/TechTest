using Projections.Core.Models;
using Test.Models;

namespace Projections.Core
{ 
    public struct AnnualGrowthFigures
    {
        public BoundedValue<double> WideBoundsPercentage { get; }
        public BoundedValue<double> NarrowBoundsPercentage { get; }

        public AnnualGrowthFigures(BoundedValue<double> wideBoundsPercentage, BoundedValue<double> narrowBoundsPercentage)
        {
            WideBoundsPercentage = wideBoundsPercentage;
            NarrowBoundsPercentage = narrowBoundsPercentage;
        }
    }

    public interface IRiskLevelAnnualGrowth
    {
        RiskLevel RiskLevel { get; }
        AnnualGrowthFigures AnnualGrowthFigures { get; }
    }

    public class LowRiskAnnualGrowthFigures : IRiskLevelAnnualGrowth
    {
        public RiskLevel RiskLevel => RiskLevel.Low;
        public AnnualGrowthFigures AnnualGrowthFigures => 
            new AnnualGrowthFigures(new BoundedValue<double>(1, 3), new BoundedValue<double>(1.5, 2.5));
    }

    public class MediumRiskAnnualGrowthFigures : IRiskLevelAnnualGrowth
    {
        public RiskLevel RiskLevel => RiskLevel.Medium;
        public AnnualGrowthFigures AnnualGrowthFigures => 
            new AnnualGrowthFigures(new BoundedValue<double>(0, 5), new BoundedValue<double>(1.5, 3.5));
    }

    public class HighRiskAnnualGrowthFigures : IRiskLevelAnnualGrowth
    {
        public RiskLevel RiskLevel => RiskLevel.High;
        public AnnualGrowthFigures AnnualGrowthFigures => 
            new AnnualGrowthFigures(new BoundedValue<double>(-1, 7), new BoundedValue<double>(2, 4));
    }
}