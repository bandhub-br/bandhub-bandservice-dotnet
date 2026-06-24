namespace BandHub.BandService.Features.Bands.Domain;

public class Band
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Genre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? SpotifyId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Band()
    {
        Name = string.Empty;
        Genre = string.Empty;
        Description = string.Empty;
    }

    public Band(Guid accountId, string name, string genre, string description, string? spotifyId = null)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Name = name;
        Genre = genre;
        Description = description;
        SpotifyId = spotifyId;
        CreatedAt = DateTime.UtcNow;
    }
}
