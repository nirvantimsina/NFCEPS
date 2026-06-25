using System.Data;
using Moq;
using NFCEPS_API.Card.Models.Request;
using NFCEPS_API.Card.Models.Response;
using NFCEPS_API.Card.Services.Implementation;
using NFCEPS_API.Repository.Interfaces;
using Npgsql;

namespace NFCEPS_TEST.Card.Services.Implementations;

public class CardServiceTests
{
    private readonly Mock<IGenericRepository> _mockRepo;
    private readonly CardService _cardService;

    public CardServiceTests()
    {
        _mockRepo = new Mock<IGenericRepository>();
        _cardService = new CardService(_mockRepo.Object);
    }

    private PostgresException CreatePostgresException(string sqlState, string message)
    {
        return new PostgresException(
            messageText: message,
            severity: "ERROR",
            invariantSeverity: "ERROR",
            sqlState: sqlState
        );
    }

    #region AssignCardAsync Tests
    [Fact]
    public async Task AssignCardAsync_ReassignCard_CardAlreadyAssigned()
    {
        // Arrange
        var request = new AssignCardRequestModel { Flag = "A", UserId = 11 };

        var exception = CreatePostgresException("P0001", "Card has already been assigned to this user!");

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
            It.IsAny<string>(),
            It.IsAny<object>(),
            It.IsAny<CommandType>()
        )).ThrowsAsync((exception));

        // Act
        var result = await _cardService.AssignCardAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Card has already been assigned to this user!", result.Message);
    }

    [Fact]
    public async Task AssignCardAsync_UserDoesNotExists_ResultUserDoesNotExists()
    {
        // Arrange
        var request = new AssignCardRequestModel { Flag = "A", UserId = 0 };

        var exception = CreatePostgresException("P0002", "Card assign failed, the user doesnot exist!");

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
            It.IsAny<string>(),
            It.IsAny<object>(),
            It.IsAny<CommandType>()
        )).ThrowsAsync((exception));

        // Act
        var result = await _cardService.AssignCardAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Card assign failed, the user doesnot exist!", result.Message);
    }

    [Fact]
    public async Task AssignCardAsync_CardAssignedToUser_ReturnsCardId()
    {
        // Arrange
        var request = new AssignCardRequestModel { Flag = "A", UserId = 11 };

        var fakeResponseData = 
    }
    #endregion
}