using System.Collections.Generic;
using Coterie.Api.Models;
using System.Threading.Tasks;

namespace Coterie.Api.Interfaces
{
    public interface IStaticFactorRepository
    {
        Task<List<Factor>> GetFactors();
    }
}