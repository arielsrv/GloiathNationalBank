using GloiathNationalBank.Services.Clients;
using GloiathNationalBank.Services.Rates.Currencies;
using log4net;
using QuikGraph;
using QuikGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GloiathNationalBank.Services.Clients.Rates;

namespace GloiathNationalBank.Services.Rates
{
    public class RateService : IRateService
    {
        /// <summary>
        /// The rate client
        /// </summary>
        private readonly IRateClient rateClient;

        /// <summary>
        /// The loaded
        /// </summary>
        private bool loaded;

        /// <summary>
        /// The rates
        /// </summary>
        private Dictionary<string, double> rates = new Dictionary<string, double>();

        /// <summary>
        /// The graph
        /// </summary>
        private readonly BidirectionalGraph<string, Edge<string>> currencies = new BidirectionalGraph<string, Edge<string>>();

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="RateService"/> class.
        /// </summary>
        /// <param name="rateClient">The rate client.</param>
        public RateService(IRateClient rateClient)
        {
            this.rateClient = rateClient;
        }

        /// <summary>
        /// Gets the rate.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public async Task<double> GetRate(Currency source, Currency target)
        {
            try
            {
                if (!loaded)
                {
                    List<RateResponse> response = await rateClient.GetRates();

                    foreach (RateResponse rateResponse in response)
                    {
                        currencies.AddVertex(rateResponse.To.ToString());
                        currencies.AddVertex(rateResponse.From.ToString());
                        currencies.AddEdge(new Edge<string>(rateResponse.From.ToString(), rateResponse.To.ToString()));
                    }

                    rates = new Dictionary<string, double>();
                    foreach (Edge<string> edge in currencies.Edges)
                    {
                        RateResponse result = response
                            .FirstOrDefault(conversion => conversion.From.ToString() == edge.Source && conversion.To.ToString() == edge.Target);

                        if (result != null) rates.Add(edge.ToString(), result.Rate);
                    }
                }

                TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths = currencies
                    .ShortestPathsDijkstra(EdgeCost, source.ToString());

                double rate = 1.0;
                if (tryGetPaths(target.ToString(), out IEnumerable<Edge<string>> path))
                {
                    rate = path.Aggregate(rate, (current, edge) => current * rates[edge.ToString()]);
                }

                loaded = true;

                return Math.Round(rate, 2, MidpointRounding.ToEven);
            }
            catch (Exception e)
            {
                logger.Error($"Source Currency: {source}, Target Currency: {target}, Error: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Edges the cost.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns></returns>
        private static double EdgeCost(Edge<string> edge)
        {
            return 1;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        /// <returns></returns>
        public async Task<List<RateDto>> GetRates()
        {
            List<RateResponse> response = await rateClient.GetRates();

            List<RateDto> result = new List<RateDto>();

            if (response != null)
            {
                foreach (RateResponse rateResponse in response)
                {
                    RateDto rateDto = new RateDto
                    {
                        From = rateResponse.From.ToString(),
                        To = rateResponse.To.ToString(),
                        Rate = rateResponse.Rate
                    };
                    result.Add(rateDto);
                }
            }

            return result;
        }
    }
}