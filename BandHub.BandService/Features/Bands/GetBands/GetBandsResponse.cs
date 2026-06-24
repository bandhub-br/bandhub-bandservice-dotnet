namespace BandHub.BandService.Features.Bands.GetBands;

public sealed record GetBandsResponse(Guid Id, Guid AccountId, string Name, string Genre, string Description, string? SpotifyId, DateTime CreatedAt);
