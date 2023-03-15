using System.Web.Http.ExceptionHandling;

namespace GloiathNationalBank.WebApi
{
    public sealed class PassthroughExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Override exception handler
        /// </summary>
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ExceptionResult(context.Exception);
        }

        /// <summary>
        /// When we should handle the error
        /// </summary>
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}