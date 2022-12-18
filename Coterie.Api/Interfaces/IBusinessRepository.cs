using System.Collections.Generic;
using Coterie.Api.Models;
using System.Threading.Tasks;


namespace Coterie.Api.Interfaces
{
    public interface IBusinessRepository
    {
        Task<Business> GetBusiness(string name);
        Task<List<Factor>> GetFactorsForBusiness(string businessName);
        Task<bool> IsValidBusinessName(string name);
    }
}