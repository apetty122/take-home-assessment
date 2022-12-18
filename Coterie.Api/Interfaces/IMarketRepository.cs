using System.Collections.Generic;
using Coterie.Api.Models;
using System.Threading.Tasks;


namespace Coterie.Api.Interfaces
{
    public interface IMarketRepository
    {
        Task<List<Factor>> GetFactorsForMarket(string marketNameOrAbbreviation);
        Task<Market> GetMarket(string marketNameOrAbbreviation);
        Task<bool> IsValidMarket(string marketNameOrAbbreviation);
    }
}