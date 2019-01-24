using Microsoft.AspNetCore.Mvc;
using Projections.Core.Calculations;
using Projections.Core.Models;
using Projections.Web.Models;

namespace Projections.Web.Controllers
{

    [Route("api/[controller]")]
    public class ForecastController : Controller
    {
        private readonly IForecastInvestments _forecastInvestments;
        private const int MonthsInYear = 12;

        public ForecastController(IForecastInvestments forecastInvestments)
        {
            _forecastInvestments = forecastInvestments;
        }


        [HttpGet("[action]")]
        public ActionResult<ForecastResult> Projection(ForecastRequest forecastRequest)
        {

            var result = _forecastInvestments.ForecastInvestment(
                                forecastRequest.LumpSumInvestment,
                                forecastRequest.MonthlyInvestment,
                                forecastRequest.TimescaleYears * MonthsInYear,
                                forecastRequest.TargetValue,
                                forecastRequest.RiskLevel);



            return new OkObjectResult(result);
        }


    }
}