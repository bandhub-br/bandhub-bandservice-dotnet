using BandHub.BandService.Features.Bands.CreateBand;
using BandHub.BandService.Features.Bands.Domain;
using FluentAssertions;
using Moq;

namespace BandHub.BandService.UnitTests.Features.Bands.CreateBand;

public class CreateBandHandlerTests
{
    private readonly Mock<IBandRepository> _bandRepositoryMock;
    private readonly CreateBandHandler _handler;
    private readonly Guid _validAccountId = Guid.NewGuid();

    public CreateBandHandlerTests()
    {
        _bandRepositoryMock = new Mock<IBandRepository>();
        _handler = new CreateBandHandler(_bandRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateBand_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "Arctic Monkeys", "Banda inglesa de indie rock", "Indie Rock", null);
        _bandRepositoryMock
            .Setup(x => x.NameExistsAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Genre.Should().Be(request.Genre);
        response.Description.Should().Be(request.Description);
        response.AccountId.Should().Be(_validAccountId);
        response.Id.Should().NotBeEmpty();

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Band>(b => b.Name == request.Name && b.Genre == request.Genre && b.AccountId == _validAccountId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "", "Uma descrição válida", "Rock", null);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Name is required*");

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenGenreIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "Arctic Monkeys", "Uma descrição válida", "", null);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Genre is required*");

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "Arctic Monkeys", "", "Indie Rock", null);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Description is required*");

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenAccountIdIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest(Guid.Empty, "Arctic Monkeys", "Descrição válida", "Indie Rock", null);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*AccountId is required*");

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenBandNameAlreadyExists()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "Arctic Monkeys", "Uma descrição válida", "Indie Rock", null);
        _bandRepositoryMock
            .Setup(x => x.NameExistsAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Band name already exists.");

        _bandRepositoryMock.Verify(
            x => x.NameExistsAsync(request.Name, It.IsAny<CancellationToken>()),
            Times.Once);

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryWithCorrectData_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateBandRequest(_validAccountId, "Tame Impala", "Projeto musical australiano", "Psychedelic Rock", "5INjqkS1o8h1imAzPqGZBb");
        Band? capturedBand = null;

        _bandRepositoryMock
            .Setup(x => x.NameExistsAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _bandRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()))
            .Callback<Band, CancellationToken>((band, _) => capturedBand = band)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        capturedBand.Should().NotBeNull();
        capturedBand!.AccountId.Should().Be(_validAccountId);
        capturedBand.Name.Should().Be(request.Name);
        capturedBand.Genre.Should().Be(request.Genre);
        capturedBand.Description.Should().Be(request.Description);
        capturedBand.Id.Should().NotBeEmpty();
        capturedBand.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenMultipleFieldsAreInvalid()
    {
        // Arrange
        var request = new CreateBandRequest(Guid.Empty, "", "", "", null);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .Where(ex => ex.Message.Contains("Name is required")
                      && ex.Message.Contains("Genre is required")
                      && ex.Message.Contains("Description is required")
                      && ex.Message.Contains("AccountId is required"));

        _bandRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
