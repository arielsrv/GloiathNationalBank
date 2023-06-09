﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace GloiathNationalBank.WebApi
{
    public sealed class ExceptionResult : IHttpActionResult
    {
        private readonly Exception exception;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExceptionResult" /> class
        /// </summary>
        public ExceptionResult(Exception exception)
        {
            while (exception.InnerException != null) exception = exception.InnerException;

            this.exception = exception;
        }

        /// <summary>
        ///     Serialize exception
        /// </summary>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new
            {
                success = false,
                errors = new[] { exception.Message },
                stack = exception.StackTrace
            };

            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8)
            };
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return Task.FromResult(responseMessage);
        }
    }
}