namespace Questionnaire_Frontend;

public static class MapRoutes
{
    public static IEndpointRouteBuilder MapTest(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/{username}/{password}", (string username, string password) =>
        {
Console.WriteLine("MapGet");
        });
        return routes;
    }
}