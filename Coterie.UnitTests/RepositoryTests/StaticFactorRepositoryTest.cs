﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api.Repositories;
using Coterie.Api.Services;
using NUnit.Framework;

namespace Coterie.UnitTests.RepositoryTests
{
    internal class StaticFactorRepositoryTest
    {
        protected StaticFactorRepository StaticFactorRepository;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            StaticFactorRepository = new StaticFactorRepository();
        }

        // test GetFactors
        [Test]
        public async Task GetFactors_ReturnsCorrectFactors()
        {
            var actual = await StaticFactorRepository.GetFactors();

            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().Name, Is.EqualTo("hazard"));
            Assert.That(actual.First().Value, Is.EqualTo(4.0));
        }
    }
}
