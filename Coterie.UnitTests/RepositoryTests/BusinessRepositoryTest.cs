using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api.Repositories;
using NUnit.Framework;

namespace Coterie.UnitTests.RepositoryTests
{
    public class BusinessRepositoryTest
    {
        protected BusinessRepository BusinessRepository;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            BusinessRepository = new BusinessRepository();
        }

        // tests for IsValidBusinessName
        [Test]
        public async Task IsValidBusinessName_WhenTheNameIsValid_ReturnsTrue()
        {
            string name = "architect";
            var actual = await BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public async Task IsValidBusinessName_WhenTheNameIsValidNotCaseSensitive_ReturnsTrue()
        {
            string name = "Programmer";
            var actual = await BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.True);
        }

        [Test]
        public async Task IsValidBusinessName_WhenTheNameIsNotValid_ReturnsFalse()
        {
            string name = "NotAValidBusinessName";
            var actual = await BusinessRepository.IsValidBusinessName(name);

            Assert.That(actual, Is.False);
        }

        // tests for GetBusiness
        [Test]
        public async Task GetBusiness_WhenTheBusinessExistsForName_ReturnsTheBusiness()
        {
            string name = "plumber";
            var actual = await BusinessRepository.GetBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("plumber"));
        }

        [Test]
        public async Task GetBusiness_WhenTheBusinessExistsForNameNotCaseSensitive_ReturnsTheBusiness()
        {
            string name = "ArChItEcT";
            var actual = await BusinessRepository.GetBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Name, Is.EqualTo("architect"));
        }

        [Test]
        public async Task GetBusiness_WhenTheBusinessNameIsNotValid_ThrowsError()
        {
            string name = "NotAValidBusinessName";

            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await BusinessRepository.GetBusiness(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No business for name: NotAValidBusinessName"));
        }

        // tests for GetFactorsForBusiness
        public async Task GetFactorsForBusiness_GetsCorrectFactors_WhenTheBusinessIsValid_ReturnsTheFactors()
        {
            string name = "programmer";
            var actual = await BusinessRepository.GetFactorsForBusiness(name);

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("BusinessFactor"));
            Assert.That(actual.First().Value, Is.EqualTo(1.25));
        }

        public async Task GetFactorsForBusiness_GetsCorrectFactors_WhenTheBusinessIsNotValid_ThrowsError()
        {
            string name = "programmer";

            Exception ex = Assert.ThrowsAsync<System.Exception>(
                async () => { await BusinessRepository.GetBusiness(name); }
            );

            Assert.That(ex.Message, Is.EqualTo("No business for name: NotAValidBusinessName"));
        }
    }
}
