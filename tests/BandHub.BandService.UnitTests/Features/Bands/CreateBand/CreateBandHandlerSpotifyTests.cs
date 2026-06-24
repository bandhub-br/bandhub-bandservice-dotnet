using BandHub.BandService.Features.Bands.CreateBand;
using BandHub.BandService.Features.Bands.Domain;
using FluentAssertions;
using Moq;

namespace BandHub.BandService.UnitTests.Features.Bands.CreateBand;

public class CreateBandHandlerSpotifyTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateBand_WhenSpotifyIdIsProvided()
    {
        var repositoryMock = new Mock<IBandRepository>();
        var handler = new CreateBandHandler(repositoryMock.Object);

        var request = new CreateBandRequest(
            Guid.NewGuid(),
            "Tame Impala",
            "Projeto musical australiano",
            "Psychedelic Rock",
            "5INjqkS1o8h1imAzPqGZBb");

        repositoryMock
            .Setup(x => x.NameExistsAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var response = await handler.HandleAsync(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.AccountId.Should().Be(request.AccountId);
    }
}
