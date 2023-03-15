﻿using GloiathNationalBank.Services.Clients;
using GloiathNationalBank.Services.Rates;
using GloiathNationalBank.Services.Rates.Currencies;
using GloiathNationalBank.Services.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using GloiathNationalBank.Services.Clients.Transactions;

namespace GloiathNationalBank.Test
{
    [TestClass]
    public class TransactionServiceTest
    {
        /// <summary>
        /// Gets the transaction by sku.
        /// </summary>
        [TestMethod]
        public void GetTransactionBySku()
        {
            Mock<ITransactionClient> transactionClient = new Mock<ITransactionClient>();
            transactionClient.Setup(x => x.GetTransactions()).ReturnsAsync(GetListResponse());

            Mock<IRateService> rateService = new Mock<IRateService>();
            rateService.Setup(x => x.GetRate(Currency.USD, Currency.EUR)).ReturnsAsync(0.736);
            rateService.Setup(x => x.GetRate(Currency.CAD, Currency.EUR)).ReturnsAsync(0.732);
            rateService.Setup(x => x.GetRate(Currency.USD, Currency.EUR)).ReturnsAsync(0.736);
            rateService.Setup(x => x.GetRate(Currency.EUR, Currency.EUR)).ReturnsAsync(1);
            rateService.Setup(x => x.GetRate(Currency.USD, Currency.EUR)).ReturnsAsync(0.736);

            ITransactionService transactionService = new TransactionService(transactionClient.Object, rateService.Object);

            SearchTransactionDTO expected = transactionService.GetTransactions("T2006").Result;

            Assert.IsTrue(expected != null);
            Assert.AreEqual(2, expected.Transactions.Count);
            Assert.AreEqual(14.99, expected.TotalAmount);
        }
        /// <summary>
        /// Gets the list response.
        /// </summary>
        /// <returns></returns>
        private List<TransactionResponse> GetListResponse()
        {
            List<TransactionResponse> responses = new List<TransactionResponse>();

            TransactionResponse transaction1 = new TransactionResponse
            {
                Sku = "T2006",
                Amount = 10.00,
                Currency = Currency.USD
            };

            TransactionResponse transaction2 = new TransactionResponse
            {
                Sku = "M2007",
                Amount = 34.57,
                Currency = Currency.CAD
            };

            TransactionResponse transaction3 = new TransactionResponse
            {
                Sku = "R2008",
                Amount = 17.95,
                Currency = Currency.USD
            };

            TransactionResponse transaction4 = new TransactionResponse
            {
                Sku = "T2006",
                Amount = 7.63,
                Currency = Currency.EUR
            };

            TransactionResponse transaction5 = new TransactionResponse
            {
                Sku = "B2009",
                Amount = 21.23,
                Currency = Currency.USD
            };

            responses.Add(transaction1);
            responses.Add(transaction2);
            responses.Add(transaction3);
            responses.Add(transaction4);
            responses.Add(transaction5);

            return responses;
        }
    }
}