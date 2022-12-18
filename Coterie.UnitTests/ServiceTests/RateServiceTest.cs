using System;
using System.Linq;
using Coterie.Api.Interfaces;
using Coterie.Api.Models;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Repositories;
using Coterie.Api.Services;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Coterie.UnitTests
{
    public class TestRateService
    {
        protected RateService RateService;
        private readonly Mock<IMarketRepository>  marketRepositoryStub = new Mock<IMarketRepository>();
        private readonly Mock<IBusinessRepository> businessRepositoryStub = new Mock<IBusinessRepository>();
        private readonly Mock<IStaticFactorRepository> staticFactorRepositoryStub = new Mock<IStaticFactorRepository>();


        // test CalculateRate
        [Test]
        public async Task CalculateRate_WhenRevenueIsInvalid_ReturnsFailureResponse ()
        {
            string businessName = "BusinessName";
            double revenue = -1;
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, new string[1]);
            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
    
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.False);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums, Is.Empty);
        }

        [Test]
        public async Task CalculateRate_WhenBusinessNameIsInvalid_ReturnsFailureResponse()
        {
            string invalidBusinessName = "InvalidBusinessName";
            double revenue = 10;
            RateRequest rateRequest = CreateRateRequest(invalidBusinessName, revenue, new string[1]);
            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(invalidBusinessName))
                .ReturnsAsync(false);

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.False);
            Assert.That(rateResponse.Business, Is.EqualTo(invalidBusinessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums, Is.Empty);
        }

        [Test]
        public async Task CalculateRate_WhenMarketsArrayIsEmpty_ReturnsFailureResponse()
        {
            string businessName = "BusinessName";
            double revenue = 10;
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, new string[] {});
            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(businessName))
                .ReturnsAsync(true);

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.False);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums, Is.Empty);
        }


        [Test]
        public async Task CalculateRate_WhenAMarketNameIsInvalid_ReturnsFailureResponse()
        {
            string businessName = "BusinessName";
            double revenue = 10;
            string invalidMarketName = "InvalidMarket";
            string[] markets = new string[] { invalidMarketName };
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, markets);
            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(businessName))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(invalidMarketName))
                .ReturnsAsync(false);

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.False);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums, Is.Empty);
        }


        [Test]
        public async Task CalculateRate_WhenOneMarketNameIsInvalid_ReturnsFailureResponse()
        {
            string businessName = "BusinessName";
            double revenue = 10;
            string invalidMarketName = "InvalidMarket";
            string validMarketName = "ValidMarketName";
            List<Factor> factors = new List<Factor> { new Factor { Name = "someFactor", Value = 1.0 } };
            string[] markets = new string[] { validMarketName, invalidMarketName };
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, markets);

            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(businessName))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(validMarketName))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(invalidMarketName))
                .ReturnsAsync(false);
            marketRepositoryStub.Setup(repo => repo.GetMarket(validMarketName))
                .ReturnsAsync(new Market() { Name = validMarketName, Abbreviation = "XX"});
            staticFactorRepositoryStub.Setup(repo => repo.GetFactors())
                .ReturnsAsync(factors);
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket(It.IsAny<string>()))
                .ReturnsAsync(factors);
            businessRepositoryStub.Setup(repo => repo.GetFactorsForBusiness(It.IsAny<string>()))
                .ReturnsAsync(factors);

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.False);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums, Is.Empty);
        }

        [Test]
        public async Task CalculateRate_WhenCalculatingPremiumForASingleMarket_CalculatesTheCorrectBasePremium()
        {
            string businessName = "BusinessName";
            double revenue = 10000;
            string validMarketName = "ValidMarketName";
            List<Factor> factors = new List<Factor> { new Factor { Name = "someFactor", Value = 1.0 } };
            string[] markets = new string[] { validMarketName };
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, markets);

            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(It.IsAny<string>()))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(It.IsAny<string>()))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.GetMarket(validMarketName))
                .ReturnsAsync(new Market() { Name = validMarketName, Abbreviation = "xx" });
            staticFactorRepositoryStub.Setup(repo => repo.GetFactors())
                .ReturnsAsync(factors);
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket(It.IsAny<string>()))
                .ReturnsAsync(factors);
            businessRepositoryStub.Setup(repo => repo.GetFactorsForBusiness(It.IsAny<string>()))
                .ReturnsAsync(factors);

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.True);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums.First().Premium, Is.EqualTo(10.0));
            Assert.That(rateResponse.Premiums.First().State, Is.EqualTo("XX"));
        }

        [Test]
        public async Task CalculateRate_WhenCalculatingPremiumForASingleMarket_CalculatesThePremiumWithFactors()
        {
            string businessName = "BusinessName";
            double revenue = 6000000;
            string validMarketName = "ValidMarketName";
            string[] markets = new string[] { validMarketName };
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, markets);

            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(It.IsAny<string>()))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(It.IsAny<string>()))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.GetMarket(validMarketName))
                .ReturnsAsync(new Market() { Name = validMarketName, Abbreviation = "xx" });
            staticFactorRepositoryStub.Setup(repo => repo.GetFactors())
                .ReturnsAsync(new List<Factor> { new Factor { Name = "hazard", Value = 4.0 } });
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket(It.IsAny<string>()))
                .ReturnsAsync(new List<Factor> { new Factor { Name = "marketFactor", Value = 0.943 } });
            businessRepositoryStub.Setup(repo => repo.GetFactorsForBusiness(It.IsAny<string>()))
                .ReturnsAsync(new List<Factor> { new Factor { Name = "businessFactor", Value = 0.5 } });

            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            Assert.That(rateResponse.IsSuccessful, Is.True);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            Assert.That(rateResponse.Premiums.First().Premium, Is.EqualTo(11316.0));
            Assert.That(rateResponse.Premiums.First().State, Is.EqualTo("XX"));
        }

        [Test]
        public async Task CalculateRate_WhenCalculatingPremiumForMultipleMarkets_CalculatesThePremiumWithFactors()
        {
            string businessName = "BusinessName";
            double revenue = 6000000;

            string market1 = "TX";
            string market2 = "OH";
            string market3 = "FLORIDA";
            string[] markets = new string[] { market1, market2, market3 };
            RateRequest rateRequest = CreateRateRequest(businessName, revenue, markets);

            businessRepositoryStub.Setup(repo => repo.IsValidBusinessName(It.IsAny<string>()))
                .ReturnsAsync(true);
            marketRepositoryStub.Setup(repo => repo.IsValidMarket(It.IsAny<string>()))
                .ReturnsAsync(true);

            marketRepositoryStub.Setup(repo => repo.GetMarket(market1))
                .ReturnsAsync(new Market() { Name = market1, Abbreviation = "TX" });
            marketRepositoryStub.Setup(repo => repo.GetMarket(market2))
                .ReturnsAsync(new Market() { Name = market1, Abbreviation = "OH" });
            marketRepositoryStub.Setup(repo => repo.GetMarket(market3))
                .ReturnsAsync(new Market() { Name = market1, Abbreviation = "FL" });

            staticFactorRepositoryStub.Setup(repo => repo.GetFactors())
                .ReturnsAsync(new List<Factor> { new Factor { Name = "hazard", Value = 4.0 } });
            businessRepositoryStub.Setup(repo => repo.GetFactorsForBusiness(It.IsAny<string>()))
                .ReturnsAsync(new List<Factor> { new Factor { Name = "businessFactor", Value = 0.5 } });
            
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket("TX"))
                .ReturnsAsync(new List<Factor> { new Factor { Name = "marketFactor", Value = 0.943 } });
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket("OH"))
                .ReturnsAsync(new List<Factor> { new Factor { Name = "marketFactor", Value = 1.0 } });
            marketRepositoryStub.Setup(repo => repo.GetFactorsForMarket("FL"))
               .ReturnsAsync(new List<Factor> { new Factor { Name = "marketFactor", Value = 1.2 } });


            RateService rateService = new RateService(marketRepositoryStub.Object, businessRepositoryStub.Object, staticFactorRepositoryStub.Object);
            RateResponse rateResponse = await rateService.CalculateRateAsync(rateRequest);

            List<Rate> expectedRates = new()
            {

                new Rate() { Premium = 11316.0, State = "TX" },
                new Rate() { Premium = 12000.0, State = "OH" },
                new Rate() { Premium = 14400.0, State = "FL"  }
             };

            Assert.That(rateResponse.IsSuccessful, Is.True);
            Assert.That(rateResponse.Business, Is.EqualTo(businessName));
            Assert.That(rateResponse.Revenue, Is.EqualTo(revenue));
            // CollectionAssert.AreEquivalent(rateResponse.Premiums, expectedRates);
            for(int i = 0; i < rateResponse.Premiums.Count; i++)
            {
                Assert.That(rateResponse.Premiums[i].State, Is.EqualTo(expectedRates[i].State));
                Assert.That(rateResponse.Premiums[i].Premium, Is.EqualTo(expectedRates[i].Premium));
            }
        }


        private RateRequest CreateRateRequest(string business, double revenue, string[] states)
        {
            return new RateRequest()
            {
                Business = business,
                Revenue = revenue,
                States = states
            };
        }
    }
}