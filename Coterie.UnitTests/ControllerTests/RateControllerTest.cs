using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api.Interfaces;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models;
using Coterie.Api.Controllers;
using Coterie.Api.Repositories;
using Coterie.Api.Services;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.UnitTests.RepositoryTests
{
    internal class RateControllerTest
    {
        protected Mock<IRateService> rateServiceStub = new Mock<IRateService>();

        // test GetRateAsync
        [Test]
        public async Task TestRateAsync_WhenTheStatusIsSuccess_ItReturnsTheResponse()
        {
            RateRequest rateRequest = new()
            {
                Business = "Plumber",
                Revenue = 6000000,
                States = new string[] { "OH" }
            };
            RateResponse expectedRateResponse = new()
            {
                IsSuccessful = true,
                Revenue = 6000000,
                TransactionId = Guid.NewGuid(),
                Premiums = new List<Rate>()
                {
                    new Rate()
                    {
                        Premium = 12000,
                        State = "OH"
                    }
                }
            };

            rateServiceStub.Setup(stub => stub.CalculateRateAsync(rateRequest))
                .ReturnsAsync(expectedRateResponse);

            RateController rateController = new RateController(rateServiceStub.Object);
            var result = await rateController.GetRateAsync(rateRequest);

            Assert.That(result.Value.IsSuccessful, Is.EqualTo(expectedRateResponse.IsSuccessful));
            Assert.That(result.Value.Revenue, Is.EqualTo(expectedRateResponse.Revenue));
            Assert.That(result.Value.TransactionId, Is.EqualTo(expectedRateResponse.TransactionId));
            Assert.That(result.Value.Premiums.First().Premium, Is.EqualTo(expectedRateResponse.Premiums.First().Premium));
            Assert.That(result.Value.Premiums.First().State, Is.EqualTo(expectedRateResponse.Premiums.First().State));

        }

        [Test]
        public async Task TestRateAsync_WhenTheStatusIsSuccess_ItReturnsABadRequest()
        {
            RateRequest rateRequest = new RateRequest()
            {
                Business = "Plumber",
                Revenue = 6000000,
                States = new string[] { "OH" }
            };
            RateResponse expectedRateResponse = new()
            {
                IsSuccessful = false,
                Revenue = 6000000,
                TransactionId = Guid.NewGuid(),
                Premiums = new List<Rate>()
            };

            rateServiceStub.Setup(stub => stub.CalculateRateAsync(rateRequest))
                .ReturnsAsync(expectedRateResponse);

            RateController rateController = new RateController(rateServiceStub.Object);
            var result = await rateController.GetRateAsync(rateRequest);
          
            Assert.That((result.Result as ObjectResult)?.StatusCode, Is.EqualTo(400));
        }


    }
}
