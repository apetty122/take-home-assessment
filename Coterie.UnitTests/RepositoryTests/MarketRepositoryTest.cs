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
        public void TestIsValidMarket_WhenFullName()
        {
            string name = "ohio";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void TestIsValidMarket_WhenFullNameNotCaseSensitive()
        {
            string name = "FLORIDA";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void TestIsValidMarket_WhenAbbreviation()
        {
            string name = "tx";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public void TestIsValidMarket_WhenAbbreviationIsNotCaseSensitive()
        {
            string name = "Oh";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.True);
        }


        [Test]
        public void TestIsValidMarket_WhenFullNameNotValid()
        {
            string name = "NotAValidName";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void TestIsValidMarket_WhenAbbreviationNotValid()
        {
            string name = "XX";
            var actual = MarketRepository.IsValidMarket(name);

            Assert.That(actual, Is.False);
        }


        // tests for GetMarket
        [Test]
        public void TestGetMarket_WhenFullNameIsValid()
        {
            string name = "texas";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("texas"));
            Assert.That(actual.Abbreviation, Is.EqualTo("tx"));
        }

        [Test]
        public void TestGetMarket_WhenFullNameIsValidNotCaseSensitive()
        {
            string name = "FlOrIda";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public void TestGetMarket_WhenAbbreviationValid()
        {
            string name = "fl";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }


        [Test]
        public void TestGetMarket_WhenAbbreviationValidNotCaseSensitive()
        {
            string name = "FL";
            var actual = MarketRepository.GetMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("florida"));
            Assert.That(actual.Abbreviation, Is.EqualTo("fl"));
        }

        [Test]
        public void TestGetMarket_WhenFullNameNotValid()
        {
            string name = "NotAValidMarketName";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        [Test]
        public void TestGetMarket_WhenAbbreviationNotValid()
        {
            string name = "XX";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }

        // tests for GetFactorsForMarket
        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheMarketFullNameIsValid()
        {
            string name = "TEXAS";
            var actual = MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheMarketAbbreviationIsValid()
        {
            string name = "TX";
            var actual = MarketRepository.GetFactorsForMarket(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("marketFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(0.943));
        }

        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheMarketFullNameIsNotValid()
        {
            string name = "NotAValidMarketName";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );
            
            Assert.That(ex.Message, Is.EqualTo("No market for name: NotAValidMarketName"));
        }

        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheMarketAbbreviationIsNotValid()
        {
            string name = "XX";

            Exception ex = Assert.Throws<System.Exception>(
                () => { MarketRepository.GetMarket(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No market for name: XX"));
        }


    }
}
