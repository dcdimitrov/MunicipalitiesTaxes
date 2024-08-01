using Microsoft.AspNetCore.Diagnostics;

namespace MunicipalitiesTaxes.Implementations
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellation)
        {
            var error = new { ErrorMessage = exception.Message };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(error, cancellation);
            return true;
        }
    }
}
