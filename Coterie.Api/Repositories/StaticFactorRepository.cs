using System.Collections.Generic;
using Coterie.Api.Interfaces;
using Coterie.Api.Models;
using System.Threading.Tasks;

namespace Coterie.Api.Repositories
{
    public class StaticFactorRepository : IStaticFactorRepository
    {
        private readonly List<Factor> staticFactors = new()
        {
            new Factor { Name = "hazard", Value = 4.0 }
        };

        public Task<List<Factor>> GetFactors()
        {
            return Task.FromResult(staticFactors);
        }

    }
}
