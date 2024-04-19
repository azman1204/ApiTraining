
namespace WebApplication1.Middleware
{
    public class MiddlewareToken : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // checking token here
            Console.WriteLine("Executing This Middleware...");
            // read token from header
            var token = context.Request.Headers.Authorization;
            Console.WriteLine($"Token = {token}");

            // this should query from DB
            //if (! (token == "123456"))
            //{
            //    // not a valid token, stop
            //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    return;
            //}
            await next(context); // run next middlware if avail
        }
    }
}
