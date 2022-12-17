using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api.Repositories;
using NUnit.Framework;

namespace Coterie.UnitTests.RepositoryTests
{
    internal class BusinessRepositoryTest
    {
        protected BusinessRepository BusinessRepository;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            BusinessRepository = new BusinessRepository();
        }

        // tests for IsValidBusinessName
        [Test]
        public void TestIsValidBusinessName_WhenTheNameIsValid()
        {
            string name = "architect";
            var actual = BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void TestIsValidBusinessName_WhenTheNameIsValidNotCaseSensitive()
        {
            string name = "Programmer";
            var actual = BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void TestIsValidBusinessName_WhenTheNameIsNotValid()
        {
            string name = "NotAValidBusinessName";
            var actual = BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.False);
        }

        // tests for GetBusiness
        [Test]
        public void TestGetBusiness_WhenTheBusinessExistsForName()
        {
            string name = "plumber";
            var actual = BusinessRepository.GetBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("plumber"));
        }

        [Test]
        public void TestGetBusiness_WhenTheBusinessExistsForNameNotCaseSensitive()
        {
            string name = "ArChItEcT";
            var actual = BusinessRepository.GetBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("architect"));
        }

        [Test]
        public void TestGetBusiness_WhenTheBusinessNameIsNotValid()
        {
            string name = "NotAValidBusinessName";

            Exception ex = Assert.Throws<System.Exception>(
                () => { BusinessRepository.GetBusiness(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No business for name: NotAValidBusinessName"));
        }

        // tests for GetFactorsForBusiness
        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheBusinessIsValid()
        {
            string name = "programmer";
            var actual = BusinessRepository.GetFactorsForBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("BusinessFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(1.25));
        }

        public void TestGetFactorsForBusiness_GetsCorrectFactors_WhenTheBusinessIsNotValid()
        {
            string name = "programmer";

            Exception ex = Assert.Throws<System.Exception>(
                () => { BusinessRepository.GetBusiness(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No business for name: NotAValidBusinessName"));
        }
    }
}
