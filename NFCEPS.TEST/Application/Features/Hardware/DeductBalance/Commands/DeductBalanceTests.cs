using Moq;
using NFCEPS.Application.Features.Hardware.DeductBalance.Commands;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using System.Data;
using Xunit;

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
            var command = new DeductBalanceCommand { CardId = 1, EntityId = 1, From = 1, Punch = 1, To = 1 };

            var expectedResponse = new StatusResponse 
            { 
                Status = "0",
                MSG = "Balance deducted successfully" 
            };
            
            _mockRepo.Setup(r => r.QueryFirstOrDefaultAsync<StatusResponse>(
                It.Is<string>(s => s.Contains("card.fn_deduct")), 
                It.IsAny<object>(), 
                It.IsAny<CommandType>()))
                .ReturnsAsync(expectedResponse); 

            // act
            var result = await _handler.Handle(command, CancellationToken.None);
            
            // assert
            Assert.False(result.IsError); 
            Assert.Equal("Balance deducted successfully", result.Value.MSG);

            _mockRepo.Verify(r => r.QueryFirstOrDefaultAsync<StatusResponse>(
                It.Is<string>(s => s.Contains("card.fn_deduct")), 
                It.IsAny<object>(),
                It.IsAny<CommandType>()),
                Times.Once
            );
        }
    }
}
