using BandHub.BandService.Features.Bands.Domain;
using BandHub.BandService.Features.Bands.GetBands;
using FluentAssertions;
using Moq;

namespace BandHub.BandService.UnitTests.Features.Bands.GetBands;

public class GetBandsHandlerTests
{
    private readonly Mock<IBandRepository> _bandRepositoryMock;
    private readonly GetBandsHandler _handler;

    public GetBandsHandlerTests()
    {
        _bandRepositoryMock = new Mock<IBandRepository>();
        _handler = new GetBandsHandler(_bandRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllBands_WhenBandsExist()
    {
        // Arrange
        var bands = new List<Band>
        {
            new(Guid.NewGuid(), "Arctic Monkeys", "Indie Rock", "Banda inglesa"),
            new(Guid.NewGuid(), "Tame Impala", "Psychedelic Rock", "Projeto australiano"),
            new(Guid.NewGuid(), "Radiohead", "Alternative Rock", "Banda inglesa experimental")
        };

        _bandRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bands);

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Arctic Monkeys");
        result[0].Genre.Should().Be("Indie Rock");
        result[1].Name.Should().Be("Tame Impala");
        result[1].Genre.Should().Be("Psychedelic Rock");
        result[2].Name.Should().Be("Radiohead");
        result[2].Genre.Should().Be("Alternative Rock");

        _bandRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoBandsExist()
    {
        // Arrange
        _bandRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Band>());

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();

        _bandRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectResponseFormat_WhenBandsExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var band = new Band(accountId, "Arctic Monkeys", "Indie Rock", "Banda inglesa de indie rock", "7Ln80lUS6He07XvHI8qqHH");
        _bandRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Band> { band });

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var response = result[0];
        response.Id.Should().Be(band.Id);
        response.AccountId.Should().Be(accountId);
        response.Name.Should().Be(band.Name);
        response.Genre.Should().Be(band.Genre);
        response.Description.Should().Be(band.Description);
        response.SpotifyId.Should().Be(band.SpotifyId);
        response.CreatedAt.Should().Be(band.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapAllBandProperties_WhenMultipleBandsExist()
    {
        // Arrange
        var band1 = new Band(Guid.NewGuid(), "Band One", "Rock", "Descrição um");
        var band2 = new Band(Guid.NewGuid(), "Band Two", "Pop", "Descrição dois", "spotify123");

        _bandRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Band> { band1, band2 });

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(band1.Id);
        result[0].AccountId.Should().Be(band1.AccountId);
        result[0].Name.Should().Be(band1.Name);
        result[0].Genre.Should().Be(band1.Genre);
        result[0].Description.Should().Be(band1.Description);
        result[0].SpotifyId.Should().BeNull();
        result[0].CreatedAt.Should().Be(band1.CreatedAt);

        result[1].Id.Should().Be(band2.Id);
        result[1].AccountId.Should().Be(band2.AccountId);
        result[1].Name.Should().Be(band2.Name);
        result[1].Genre.Should().Be(band2.Genre);
        result[1].Description.Should().Be(band2.Description);
        result[1].SpotifyId.Should().Be("spotify123");
        result[1].CreatedAt.Should().Be(band2.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryOnce_WhenCalled()
    {
        // Arrange
        _bandRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Band>());

        // Act
        await _handler.HandleAsync(CancellationToken.None);

        // Assert
        _bandRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _bandRepositoryMock.VerifyNoOtherCalls();
    }
}
