using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Coterie.Api.Models;

namespace Coterie.Api.Repositories
{
    public class MarketRepository
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
    }
}
