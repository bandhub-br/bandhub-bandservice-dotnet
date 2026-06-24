namespace BandHub.BandService.Features.Bands.Domain;

public interface IBandRepository
{
    Task AddAsync(Band band, CancellationToken cancellationToken);
    Task<List<Band>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken);
    Task<bool> AccountIdExists(Guid accountId, CancellationToken cancellationToken);
}
