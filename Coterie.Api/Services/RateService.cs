using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coterie.Api.Interfaces;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Repositories;
using Coterie.Api.Models;
using Coterie.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.Api.Services
{
    public class RateService : IRateService
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IStaticFactorRepository _staticFactorRepository;

        public RateService(IMarketRepository marketRepository, IBusinessRepository businessRepository, IStaticFactorRepository staticFactorRepository) {
            _marketRepository = marketRepository;
            _businessRepository = businessRepository;
            _staticFactorRepository = staticFactorRepository;
        }

        public async Task<RateResponse> CalculateRateAsync(RateRequest rateRequest)
        {
            string businessRequest = rateRequest.Business;
            double revenueRequest = rateRequest.Revenue;

            RateResponse rateResponse = new()
            {
                Business = businessRequest,
                Revenue = revenueRequest,
                Premiums = new List<Rate>(),
                IsSuccessful = false,
                TransactionId = Guid.NewGuid(),
            };

            bool isValidRevenue = revenueRequest > 0;
            bool isValidBusinessName = await _businessRepository.IsValidBusinessName(businessRequest);
            string[] markets = rateRequest.States;

            if ( !isValidRevenue || !isValidBusinessName || markets.Length == 0 )
            {
                return rateResponse;
            }

            foreach ( string marketName in markets )
            {
                bool isValidMarketName = await _marketRepository.IsValidMarket(marketName);

                if ( !isValidMarketName )
                {
                    rateResponse.Premiums = new List<Rate>(); // clear the list of premiums
                    return rateResponse;
                }

                Market market = await _marketRepository.GetMarket(marketName);
                Rate rate = await CalculatePremiums(revenueRequest, businessRequest, market.Abbreviation);
                rateResponse.Premiums.Add(rate);
            }

            rateResponse.IsSuccessful= true;
            return rateResponse;
        }

        private async Task<Rate> CalculatePremiums(double revenue, string businessName, string marketAbbreviation)
        {
            double basePremium = Math.Ceiling(revenue / 1000);
            List<Factor> staticFactors = await _staticFactorRepository.GetFactors();
            List<Factor> marketFactors = await _marketRepository.GetFactorsForMarket(marketAbbreviation);
            List<Factor> businessFactors = await _businessRepository.GetFactorsForBusiness(businessName);

            List<Factor> factors =
                staticFactors
                .Concat(marketFactors)
                .Concat(businessFactors)
                .ToList();

            double totalPremium = basePremium;
            foreach (Factor factor in factors ) {
                totalPremium *= factor.Value;
            }

            return new Rate()
            {
                Premium = totalPremium,
                State = marketAbbreviation.ToUpper(),
            };
        }
    }
}