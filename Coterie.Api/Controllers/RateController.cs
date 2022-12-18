using System.Net.Http;
using System.Threading.Tasks;
using Coterie.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Coterie.Api.Models.Requests;
using Coterie.Api.Services;
using Coterie.Api.Repositories;
using Coterie.Api.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Coterie.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateController : CoterieBaseController
    {
        private readonly IRateService _rateService;
        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpPost]
        public async Task<ActionResult<RateResponse>> GetRateAsync(RateRequest rateRequest)
        {
            RateResponse rateResult = await _rateService.CalculateRateAsync(rateRequest);

            if (!rateResult.IsSuccessful)
            {
                return BadRequest();
            }

            return rateResult;
        }
    }
}