using System;
using System.Collections.Generic;

namespace Coterie.Api.Models.Responses
{
    public class RateResponse
    {
        public string Business { get; set; }
        public double Revenue { get; set; }
        public List<Rate> Premiums { get; set; }
        public bool IsSuccessful { get; set; }
        public Guid TransactionId { get; set; }

    }
}