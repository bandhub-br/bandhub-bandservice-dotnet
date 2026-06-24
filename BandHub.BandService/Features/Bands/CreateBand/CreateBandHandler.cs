using BandHub.BandService.Features.Bands.Domain;

namespace BandHub.BandService.Features.Bands.CreateBand;

public class CreateBandHandler
{
    private readonly IBandRepository _bandRepository;

    public CreateBandHandler(IBandRepository bandRepository)
    {
        _bandRepository = bandRepository;
    }

    public async Task<CreateBandResponse> HandleAsync(CreateBandRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateBandValidator();
        var errors = validator.Validate(request);
        if (errors.Count != 0)
            throw new ArgumentException(string.Join(" ", errors));
        var nameExists = await _bandRepository.NameExistsAsync(request.Name, cancellationToken);
        if (nameExists)
            throw new InvalidOperationException("Band name already exists.");
        var band = new Band(request.AccountId, request.Name, request.Genre, request.Description);
        await _bandRepository.AddAsync(band, cancellationToken);
        return new CreateBandResponse(band.Id, band.AccountId, band.Name, band.Genre, band.Description, band.SpotifyId, band.CreatedAt);
    }
}
