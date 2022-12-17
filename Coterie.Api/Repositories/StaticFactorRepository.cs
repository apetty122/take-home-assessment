﻿using System.Collections.Generic;
using Coterie.Api.Models;

namespace Coterie.Api.Repositories
{
    public class StaticFactorRepository
    {
        private readonly List<Factor> staticFactors = new()
        {
            new Factor { Name = "hazard", Value = 4.0 }
        };

    }
}
