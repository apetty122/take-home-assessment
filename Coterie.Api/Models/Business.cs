using System.Collections.Generic;

namespace Coterie.Api.Models
{
    public class Business
    {
        public string Name { get; set; }
        public List<Factor> Factors { get; set; }
    }
}
