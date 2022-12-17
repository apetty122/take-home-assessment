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
        public void IsValidMarket_WhenFullName_ReturnsTrue()
        {
            string name = "ohio";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void IsValidMarket_WhenFullNameNotCaseSensitive_ReturnsTrue()
        {
            string name = "FLORIDA";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void IsValidMarket_WhenAbbreviation_ReturnsTrue()
        {
            string name = "tx";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public void IsValidMarket_WhenAbbreviationIsNotCaseSensitive_ReturnsTrue()
        {
            string name = "Oh";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public void IsValidMarket_WhenFullNameNotValid_ReturnsFalse()
        {
            string name = "NotAValidName";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void IsValidMarket_WhenAbbreviationNotValid_ReturnsFalse()
        {
            string name = "XX";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }


        // tests for GetMarket
        [Test]
        public void GetMarket_WhenFullNameIsValid_ReturnsTheMarket()
        {
            string name = "texas";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("texas"));
            Assert.That(actual.Abbreviation, Is.EqualTo("tx"));
        }

        [Test]
        public void GetMarket_WhenFullNameIsValidNotCaseSensitive_ReturnsTheMarket()
        {
            string name = "FlOrIda";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public void GetMarket_WhenAbbreviationValid_ReturnsTheMarket()
        {
            string name = "fl";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }


        [Test]
        public void GetMarket_WhenAbbreviationValidNotCaseSensitive_ReturnsTheMarket()
        {
            string name = "FL";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public void GetMarket_WhenFullNameNotValid_ThrowsError()
        {
            string name = "NotAValidMarketName";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        [Test]
        public void GetMarket_WhenAbbreviationNotValid_ThrowsError()
        {
            string name = "XX";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }

        // tests for GetFactorsForMarket
        public void TestGetFactorsForBusiness_WhenTheMarketFullNameIsValid_GetsCorrectFactors()
        {
            string name = "TEXAS";
            var actual = MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public void TestGetFactorsForBusiness_WhenTheMarketAbbreviationIsValid_GetsCorrectFactors()
        {
            string name = "TX";
            var actual = MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public void TestGetFactorsForBusiness_WhenTheMarketFullNameIsNotValid_GetsCorrectFactors()
        {
            string name = "NotAValidMarketName";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        public void TestGetFactorsForBusiness_WhenTheMarketAbbreviationIsNotValid_GetsCorrectFactors()
        {
            string name = "XX";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }


    }
}
