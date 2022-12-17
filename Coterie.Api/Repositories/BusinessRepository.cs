using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Coterie.Api.Interfaces;
using Coterie.Api.Models;

namespace Coterie.Api.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {

        // we are assuming here that Name acts as a unique identifier for the business
        private readonly List<Business> businesses = new()
        {
          new Business {
             Name = "architect",
             Factors =  new List<Factor> { new Factor { Name = "businessFactor", Value = 1 } }
          },
          new Business {
             Name = "plumber",
             Factors =  new List<Factor> { new Factor { Name = "businessFactor", Value = 0.5 } }
          },
          new Business {
             Name = "programmer",
             Factors =  new List<Factor> { new Factor { Name = "businessFactor", Value = 1.25 } }
          }
        };

        public bool IsValidBusinessName(string name)
        {
            return businesses.Any(business => business.Name.ToLower() == name.ToLower());

        }

        public Business GetBusiness(string name)
        {
            Business business = businesses.SingleOrDefault(business => business.Name.ToLower() == name.ToLower());

            if (business == null)
            {
                throw new System.Exception(string.Format("No business for name: {0}", name));
            }

            return business;
        }

        public List<Factor> GetFactorsForBusiness(string businessName)
        {
            return GetBusiness(businessName).Factors;
        }

    }
}
