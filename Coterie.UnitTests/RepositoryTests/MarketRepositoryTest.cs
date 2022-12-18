using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api.Repositories;
using NUnit.Framework;

namespace Coterie.UnitTests.RepositoryTests
{
    internal class MarketRepositoryTest
    {
        protected MarketRepository MarketRepository;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            MarketRepository = new MarketRepository();
        }

        // tests for IsValidMarket
        [Test]
        public async Task IsValidMarket_WhenFullName_ReturnsTrue()
        {
            string name = "ohio";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public async Task IsValidMarket_WhenFullNameNotCaseSensitive_ReturnsTrue()
        {
            string name = "FLORIDA";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public async Task IsValidMarket_WhenAbbreviation_ReturnsTrue()
        {
            string name = "tx";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public async Task IsValidMarket_WhenAbbreviationIsNotCaseSensitive_ReturnsTrue()
        {
            string name = "Oh";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public async Task IsValidMarket_WhenFullNameNotValid_ReturnsFalse()
        {
            string name = "NotAValidName";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }

        [Test]
        public async Task IsValidMarket_WhenAbbreviationNotValid_ReturnsFalse()
        {
            string name = "XX";
            var actual = await MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }


        // tests for GetMarket
        [Test]
        public async Task GetMarket_WhenFullNameIsValid_ReturnsTheMarket()
        {
            string name = "texas";
            var actual = await MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("texas"));
            Assert.That(actual.Abbreviation, Is.EqualTo("tx"));
        }

        [Test]
        public async Task GetMarket_WhenFullNameIsValidNotCaseSensitive_ReturnsTheMarket()
        {
            string name = "FlOrIda";
            var actual = await MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public async Task GetMarket_WhenAbbreviationValid_ReturnsTheMarket()
        {
            string name = "fl";
            var actual = await MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }


        [Test]
        public async Task GetMarket_WhenAbbreviationValidNotCaseSensitive_ReturnsTheMarket()
        {
            string name = "FL";
            var actual = await MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public async Task GetMarket_WhenFullNameNotValid_ThrowsError()
        {
            string name = "NotAValidMarketName";

            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        [Test]
        public async Task GetMarket_WhenAbbreviationNotValid_ThrowsError()
        {
            string name = "XX";

            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await MarketRepository.GetMarket(name); }
            );


            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }

        // tests for GetFactorsForMarket
        public async Task TestGetFactorsForBusiness_WhenTheMarketFullNameIsValid_GetsCorrectFactors()
        {
            string name = "TEXAS";
            var actual = await MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public async Task TestGetFactorsForBusiness_WhenTheMarketAbbreviationIsValid_GetsCorrectFactors()
        {
            string name = "TX";
            var actual = await MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public async Task TestGetFactorsForBusiness_WhenTheMarketFullNameIsNotValid_GetsCorrectFactors()
        {
            string name = "NotAValidMarketName";


            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        public async Task TestGetFactorsForBusiness_WhenTheMarketAbbreviationIsNotValid_GetsCorrectFactors()
        {
            string name = "XX";

            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await MarketRepository.GetMarket(name); }
            );


            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }


    }
}
