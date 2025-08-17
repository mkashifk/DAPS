using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedKernel.Models;
using System.Threading.Tasks;

namespace SharedKernel.Filters
{
    public class ApiResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                // If already wrapped, just continue
                if (objectResult.Value is ApiResponse<object>)
                {
                    await next();
                    return;
                }

                // Determine actual status code or default to 200
                var statusCode = objectResult.StatusCode ?? 200;

                // Get message based on status code
                var message = GetDefaultMessageForStatusCode(statusCode);

                var wrappedResponse = new ApiResponse<object>(objectResult.Value, statusCode, message);

                context.Result = new ObjectResult(wrappedResponse)
                {
                    StatusCode = statusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                var wrappedResponse = new ApiResponse<object?>(null, 204, "No Content");

                context.Result = new ObjectResult(wrappedResponse)
                {
                    StatusCode = 204
                };
            }

            await next();
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };
        }
    }
}
