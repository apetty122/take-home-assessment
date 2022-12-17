using System.Collections.Generic;
using Coterie.Api.Models;

namespace Coterie.Api.Interfaces
{
    public interface IBusinessRepository
    {
        Business GetBusiness(string name);
        List<Factor> GetFactorsForBusiness(string businessName);
        bool IsValidBusinessName(string name);
    }
}