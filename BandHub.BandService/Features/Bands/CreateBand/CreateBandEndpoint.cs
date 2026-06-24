using Microsoft.AspNetCore.Mvc;

namespace BandHub.BandService.Features.Bands.CreateBand;

public static class CreateBandEndpoint
{
    public static IEndpointRouteBuilder MapCreateBandEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bands", async (
            [FromBody] CreateBandRequest request,
            CreateBandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/bands/{response.Id}", response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
        })
        .WithName("CreateBand")
        .WithTags("Bands");
        return app;
    }
}
