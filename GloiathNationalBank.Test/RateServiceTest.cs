using GloiathNationalBank.Services.Clients;
using GloiathNationalBank.Services.Rates;
using GloiathNationalBank.Services.Rates.Currencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using GloiathNationalBank.Services.Clients.Rates;

namespace GloiathNationalBank.Test
{
    [TestClass]
    public class RateServiceTest
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        [TestMethod]
        public void GetRates()
        {
            Mock<IRateClient> rateClient = new Mock<IRateClient>();
            rateClient.Setup(x => x.GetRates()).ReturnsAsync(GetListResponse());

            IRateService rateService = new RateService(rateClient.Object);

            double expected = rateService.GetRate(Currency.EUR, Currency.USD).Result;

            Assert.AreEqual(expected, 1.36);
        }

        /// <summary>
        /// Gets the missing rates.
        /// </summary>
        [TestMethod]
        public void GetMissingRates()
        {
            Mock<IRateClient> rateClient = new Mock<IRateClient>();
            rateClient.Setup(x => x.GetRates()).ReturnsAsync(GetListResponse());

            IRateService rateService = new RateService(rateClient.Object);

            double expected = rateService.GetRate(Currency.USD, Currency.CAD).Result;

            Assert.AreEqual(expected, 1.01);
        }

        /// <summary>
        /// Gets the list response.
        /// </summary>
        /// <returns></returns>
        private List<RateResponse> GetListResponse()
        {
            List<RateResponse> responses = new List<RateResponse>();

            RateResponse rate1 = new RateResponse
            {
                From = Currency.EUR,
                To = Currency.USD,
                Rate = 1.359
            };

            RateResponse rate2 = new RateResponse
            {
                From = Currency.CAD,
                To = Currency.EUR,
                Rate = 0.732
            };

            RateResponse rate3 = new RateResponse
            {
                From = Currency.USD,
                To = Currency.EUR,
                Rate = 0.736
            };

            RateResponse rate4 = new RateResponse
            {
                From = Currency.EUR,
                To = Currency.CAD,
                Rate = 1.366
            };

            responses.Add(rate1);
            responses.Add(rate2);
            responses.Add(rate3);
            responses.Add(rate4);

            return responses;
        }
    }
}