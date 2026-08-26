using Moq;
using NFCEPS.Application.Features.Hardware.DeductBalance.Commands;
using NFCEPS.Application.Interfaces;
using System.Data;

namespace NFCEPS.TEST.Application.Features.Hardware.DeductBalance.Commands
{
    public class DeductBalanceTests
    {
        private readonly Mock<IGenericRepository> _mockRepo;
        private readonly DeductBalanceCommandHandler _handler;

        public DeductBalanceTests()
        {
            _mockRepo = new Mock<IGenericRepository>();
            _handler = new DeductBalanceCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_DeductBalance_BalanceDeducted()
        {
            // arrange
            var commmand = new DeductBalanceCommand { CardId = 1, EntityId = 1, From = 1, Punch = 1, To = 1 };

            _mockRepo.Setup(r => r.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()));

            var handler = new DeductBalanceCommandHandler(_mockRepo.Object);

            // act
            var result = await handler.Handle(commmand, CancellationToken.None);
            
            // assert
            Assert.True(result.Success);
            Assert.Equal("Balance deducted successfullt", result.Message);

            _mockRepo.Verify(r => r.ExecuteAsync(
                It.Is<string>(s => s == "sp_deductbalance" || s.Contains("deductbalance")), // Check procedure name
                It.IsAny<object>(),
                It.IsAny<CommandType>()),
                Times.Once
            );
        }
    }
}
