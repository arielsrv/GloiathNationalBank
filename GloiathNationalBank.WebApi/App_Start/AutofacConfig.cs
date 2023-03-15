using System.Configuration;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GloiathNationalBank.Services.Clients.Rates;
using GloiathNationalBank.Services.Clients.Transactions;
using GloiathNationalBank.Services.Rates;
using GloiathNationalBank.Services.Transactions;
using GloiathNationalBank.Storage;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;
using static System.Reflection.Assembly;
using static System.Web.Mvc.DependencyResolver;

namespace GloiathNationalBank.WebApi.App_Start
{
    public static class AutofacConfig
    {
        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        private static IContainer Container { get; set; }

        /// <summary>
        ///     Configures the container.
        /// </summary>
        public static void ConfigureContainer()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            Build(containerBuilder);

            // Register dependencies in controllers
            containerBuilder.RegisterControllers(GetExecutingAssembly());
            containerBuilder.RegisterApiControllers(GetExecutingAssembly());

            // Register dependencies in filter attributes
            containerBuilder.RegisterFilterProvider();

            // Register dependencies in custom views
            containerBuilder.RegisterSource(new ViewRegistrationSource());

            Container = containerBuilder.Build();

            // Set MVC DI resolver to use our Autofac container
            SetResolver(new AutofacDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        private static void Build(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(serializer => new NewtonsoftSerializer(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include
            })).As<ISerializer>().SingleInstance();

            containerBuilder.Register(cacheClient =>
                {
                    string connectionString = ConfigurationManager
                        .AppSettings
                        .Get("StorageProvider.ConnectionString");

                    RedisConfiguration redisConfiguration = new RedisConfiguration
                    {
                        ConnectionString = connectionString
                    };
                    IRedisCacheConnectionPoolManager redisCacheConnectionPoolManager =
                        new RedisCacheConnectionPoolManager(redisConfiguration);
                    ISerializer serializer = Container.Resolve<ISerializer>();
                    return new RedisCacheClient(redisCacheConnectionPoolManager, serializer, redisConfiguration);
                })
                .As<IRedisCacheClient>().SingleInstance();

            containerBuilder.Register<IStorageProvider>(
                    cacheClient => new StorageProvider(Container.Resolve<IRedisCacheClient>()))
                .As<IStorageProvider>().SingleInstance();

            containerBuilder.Register(rateClient => new RateClient(Container.Resolve<IStorageProvider>()))
                .As<IRateClient>().SingleInstance();

            containerBuilder.Register(transactionClient => new TransactionClient())
                .As<ITransactionClient>().SingleInstance();

            containerBuilder.Register(rateService => new RateService(Container.Resolve<IRateClient>()))
                .As<IRateService>().SingleInstance();

            containerBuilder.Register(transactionService =>
                    new TransactionService(Container.Resolve<ITransactionClient>(), Container.Resolve<IRateService>()))
                .As<ITransactionService>().SingleInstance();
        }
    }
}