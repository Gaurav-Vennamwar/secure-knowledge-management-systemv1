using System.Net;
using System.Text.Json;
using SecureKnowledgeManagementSystemv1.API.Models.Wrappers;



namespace SecureKnowledgeManagementSystemv1.API.Middlewares
    {
        public class GlobalExceptionMiddleware
        {
            // next = the next middleware in the pipeline
            private readonly RequestDelegate next;
            // logger = to log the error
            private readonly ILogger<GlobalExceptionMiddleware> logger;

            public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
            {
                this.next = next;
                this.logger = logger;
            }

            public async Task InvokeAsync(HttpContext httpContext)
            {
                try
                {
                    // try to pass request to next middleware
                    await next(httpContext);
                }
                catch (Exception ex)
                {
                    // something crashed — we catch it here
                    logger.LogError(ex, ex.Message);
                    await HandleExceptionAsync(httpContext, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
                // set response type to JSON
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // clean error response
            var error = ApiResponse<object>.FailureResponse("Somethingh Went Wrong On Our End. Please Try Again Later", 500);

                return context.Response.WriteAsync(JsonSerializer.Serialize(error));
            }
        }
    }
}

