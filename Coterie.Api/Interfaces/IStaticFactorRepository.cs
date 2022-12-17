using System.Collections.Generic;
using Coterie.Api.Models;

namespace Coterie.Api.Interfaces
{
    public interface IStaticFactorRepository
    {
        List<Factor> GetFactors();
    }
}