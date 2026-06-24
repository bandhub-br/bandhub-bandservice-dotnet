using BandHub.BandService.Features.Bands.Domain;
using Microsoft.EntityFrameworkCore;

namespace BandHub.BandService.Infrastructure.Persistence;

public class BandRepository : IBandRepository
{
    private readonly BandDbContext _context;

    public BandRepository(BandDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AccountIdExists(Guid accountId, CancellationToken cancellationToken)
    {
        return await _context.Bands
            .AnyAsync(x => x.AccountId == accountId, cancellationToken);
    }

    public async Task AddAsync(Band band, CancellationToken cancellationToken)
    {
        await _context.Bands.AddAsync(band, cancellationToken);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Band>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Bands
                        .OrderBy(x => x.CreatedAt)
                        .ToListAsync(cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Bands
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
