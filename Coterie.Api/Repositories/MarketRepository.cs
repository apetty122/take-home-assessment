using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Coterie.Api.Interfaces;
using Coterie.Api.Models;
using System.Threading.Tasks;


namespace Coterie.Api.Repositories
{
    public class MarketRepository : IMarketRepository
    {
        private readonly List<Market> markets = new() {
            new Market {
               Name = "ohio",
               Abbreviation = "oh",
               Factors = new List<Factor> { new Factor { Name = "marketFactor", Value = 1 } }
            },
            new Market {
               Name = "florida",
               Abbreviation = "fl",
               Factors = new List<Factor> { new Factor { Name = "marketFactor", Value = 1.2 } }
            },
            new Market {
               Name = "texas",
               Abbreviation = "tx",
               Factors = new List<Factor> { new Factor { Name = "marketFactor", Value = 0.943 } }
            }
        };

        public async Task<bool> IsValidMarket(string marketNameOrAbbreviation)
        {
            bool isValidMarket = !!markets.Any(market => market.Name.ToLower() == marketNameOrAbbreviation.ToLower() || market.Abbreviation.ToLower() == marketNameOrAbbreviation.ToLower());
            return await Task.FromResult(isValidMarket);
        }

        public async Task<Market> GetMarket(string marketNameOrAbbreviation)
        {
            Market market = marketNameOrAbbreviation.Length == 2 ? ( await GetMarketByAbbreviation(marketNameOrAbbreviation)) : (await GetMarketByName(marketNameOrAbbreviation));


            if (market == null)
            {
                throw new System.Exception(string.Format("No market for name: {0}", marketNameOrAbbreviation));
            }

            return market;
        }

        public async Task<List<Factor>> GetFactorsForMarket(string marketNameOrAbbreviation)
        {
            return ( await GetMarket(marketNameOrAbbreviation)).Factors;
        }

        private async Task<Market> GetMarketByName(string name)
        {
            Market market = markets.SingleOrDefault(market => market.Name.ToLower() == name.ToLower());
            return await Task.FromResult(market);
        }

        private async Task<Market> GetMarketByAbbreviation(string abbreviation)
        {
            Market market = markets.SingleOrDefault(market => market.Abbreviation.ToLower() == abbreviation.ToLower());
            return await Task.FromResult(market);
        }

    }
}
