namespace BandHub.BandService.Features.Bands.GetBands;

public static class GetBandsEndpoint
{
    public static IEndpointRouteBuilder MapGetBandsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/bands", async (
            GetBandsHandler handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(cancellationToken);
            return Results.Ok(response);
        })
        .WithName("GetBands")
        .WithTags("Bands");
        return app;
    }
}
