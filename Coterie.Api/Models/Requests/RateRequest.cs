using System;
using System.Collections.Generic;

namespace Coterie.Api.Models.Requests
{
    public class RateRequest
    {
        public string Business { get; set; }
        public double Revenue { get; set; }
        public string[] States { get; set; }
    }
}