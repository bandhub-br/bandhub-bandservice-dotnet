using BandHub.BandService.Features.Bands.Domain;

namespace BandHub.BandService.Features.Bands.GetBands;

public class GetBandsHandler
{
    private readonly IBandRepository _bandRepository;
    public GetBandsHandler(IBandRepository bandRepository)
    {
        _bandRepository = bandRepository;
    }
    public async Task<List<GetBandsResponse>> HandleAsync(CancellationToken cancellationToken)
    {
        var bands = await _bandRepository.GetAllAsync(cancellationToken);
        return bands
            .Select(band => new GetBandsResponse(band.Id, band.AccountId, band.Name, band.Genre, band.Description, band.SpotifyId, band.CreatedAt))
            .ToList();
    }
}
