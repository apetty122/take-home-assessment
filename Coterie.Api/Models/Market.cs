using System.Collections.Generic;

namespace Coterie.Api.Models
{
    // the name market is chosen instead of state for expandability purposes
    // for example Washington DC and outside the US
    public class Market
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<Factor> Factors { get; set; }
    }
}
