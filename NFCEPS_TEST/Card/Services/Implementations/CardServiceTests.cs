using System.Data;
using Moq;
using NFCEPS.Application.Models.Card.Response;
using NFCEPS.Application.Features.Card.Commands.AssignCard;
using NFCEPS.Application.Interfaces;
using Npgsql;

namespace NFCEPS_TEST.Card.Services.Implementations;

public class CardServiceTests
{
    private readonly Mock<IGenericRepository> _mockRepo;
    private readonly AssignCardCommandHandler _handler;

    public CardServiceTests()
    {
        _mockRepo = new Mock<IGenericRepository>();
        _handler = new AssignCardCommandHandler(_mockRepo.Object);
    }

    private PostgresException CreatePostgresException(string sqlState, string message)
    {
        return new PostgresException(message, "ERROR", "ERROR", sqlState);
    }

    [Fact]
    public async Task AssignCardAsync_ReassignCard_CardAlreadyAssigned()
    {
        var command = new AssignCardCommand { Flag = "A", UserId = 11 };
        var exception = CreatePostgresException("P0001", "Card has already been assigned to this user!");

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ThrowsAsync(exception);

        var result = await _handler.Handle(command, default);

        Assert.False(result.Success);
        Assert.Equal("Card has already been assigned to this user!", result.Message);
    }

    [Fact]
    public async Task AssignCardAsync_UserDoesNotExists_ResultUserDoesNotExists()
    {
        var command = new AssignCardCommand { Flag = "A", UserId = 0 };
        var exception = CreatePostgresException("P0002", "Card assign failed, the user doesnot exist!");

        _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
        )).ThrowsAsync(exception);

        var result = await _handler.Handle(command, default);

        Assert.False(result.Success);
        Assert.Equal("Card assign failed, the user doesnot exist!", result.Message);
    }
}
