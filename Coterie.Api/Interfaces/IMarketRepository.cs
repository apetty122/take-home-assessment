using System.Collections.Generic;
using Coterie.Api.Models;

namespace Coterie.Api.Interfaces
{
    public interface IMarketRepository
    {
        List<Factor> GetFactorsForMarket(string marketNameOrAbbreviation);
        Market GetMarket(string marketNameOrAbbreviation);
        bool IsValidMarket(string marketNameOrAbbreviation);
    }
}