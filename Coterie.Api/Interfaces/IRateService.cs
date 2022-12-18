using System.Collections.Generic;
using System.Threading.Tasks;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Requests;

namespace Coterie.Api.Interfaces
{
    public interface IRateService
    {
      Task<RateResponse> CalculateRateAsync(RateRequest rateRequest);
    }
}