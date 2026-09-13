using System.Data;
using Moq;
using NFCEPS.Application.Features.Card.Commands.AssignCard;
using NFCEPS.Application.Interfaces;

namespace NFCEPS.TEST.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardTests
    {
        private readonly Mock<IGenericRepository> _mockRepo;
        private readonly AssignCardCommandHandler _handler;

        public AssignCardTests()
        {
            _mockRepo = new Mock<IGenericRepository>();
            _handler = new AssignCardCommandHandler(_mockRepo.Object);
        }


        [Fact]
        public async Task AssignCardAsync_ReassignCard_CardAlreadyAssigned()
        {
            var command = new AssignCardCommand { UserId = 11 };
            var expectedResponse = new AssignCardResponseModel { Status = "1", MSG = "Card has already been assigned to this user!" };

            _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
            )).ReturnsAsync(expectedResponse);

            var result = await _handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal("Card has already been assigned to this user!", result.FirstError.Description);
        }

        [Fact]
        public async Task AssignCardAsync_UserDoesNotExists_ResultUserDoesNotExists()
        {
            var command = new AssignCardCommand { UserId = 0 };
            var expectedResponse = new AssignCardResponseModel { Status = "1", MSG = "Card assign failed, the user doesnot exist!" };

            _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()
            )).ReturnsAsync(expectedResponse);

            var result = await _handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal("Card assign failed, the user doesnot exist!", result.FirstError.Description);
        }
    }
}