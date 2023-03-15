using System.Reflection;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace GloiathNationalBank.WebApi.Common.ErrorHandling
{
    public class SimpleExceptionLogger : ExceptionLogger
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     When overridden in a derived class, logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error(context.Exception);
        }
    }
}