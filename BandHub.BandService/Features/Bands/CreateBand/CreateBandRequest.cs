namespace BandHub.BandService.Features.Bands.CreateBand;

public sealed record CreateBandRequest(Guid AccountId, string Name, string Description, string Genre, string? SpotifyId);