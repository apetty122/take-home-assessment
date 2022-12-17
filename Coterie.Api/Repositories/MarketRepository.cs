using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Coterie.Api.Interfaces;
using Coterie.Api.Models;

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

        public bool IsValidMarket(string marketNameOrAbbreviation)
        {
            return !!markets.Any(market => market.Name.ToLower() == marketNameOrAbbreviation.ToLower() || market.Abbreviation.ToLower() == marketNameOrAbbreviation.ToLower());
        }

        public Market GetMarket(string marketNameOrAbbreviation)
        {
            Market market = marketNameOrAbbreviation.Length == 2 ? GetMarketByAbbreviation(marketNameOrAbbreviation) : GetMarketByName(marketNameOrAbbreviation);


            if (market == null)
            {
                throw new System.Exception(string.Format("No market for name: {0}", marketNameOrAbbreviation));
            }

            return market;
        }

        public List<Factor> GetFactorsForMarket(string marketNameOrAbbreviation)
        {
            return GetMarket(marketNameOrAbbreviation).Factors;
        }

        private Market GetMarketByName(string name)
        {
            return markets.SingleOrDefault(market => market.Name.ToLower() == name.ToLower());

        }

        private Market GetMarketByAbbreviation(string abbreviation)
        {
            return markets.SingleOrDefault(market => market.Abbreviation.ToLower() == abbreviation.ToLower());
        }

    }
}
