namespace WebApplication1.Handlers
{
    public class PeopleHandler
    {
        public static void MapEndPoints(IEndpointRouteBuilder app)
        {
            // define all routes
            app.MapGet("/api/people", GetList);
            app.MapPost("/api/people", Insert);
        }

        private static IResult GetList()
        {
            return Results.Ok("list..");
        }

        private static IResult Insert()
        {
            return Results.Ok("insert..");
        }
    }
}
