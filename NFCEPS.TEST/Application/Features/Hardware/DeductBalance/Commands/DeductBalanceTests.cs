using Moq;
using NFCEPS.Application.Features.Hardware.DeductBalance.Commands;
using NFCEPS.Application.Interfaces;
using NFCEPS.Shared.Wrappers; // Ensure you have access to your ErrorCodes if needed
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
            // Centralized initialization (removed the redundant instantiation inside the test method)
            _handler = new DeductBalanceCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_DeductBalance_BalanceDeducted()
        {
            // arrange
            var command = new DeductBalanceCommand { CardId = 1, EntityId = 1, From = 1, Punch = 1, To = 1 };

            // 1. Fix: Repository framework methods usually return Task<int>. Ensure the mock returns something.
            // Use .Returns(Task.CompletedTask) for void async Task methods
            _mockRepo.Setup(r => r.ExecuteAsync(
                It.IsAny<string>(), 
                It.IsAny<object>(), 
                It.IsAny<CommandType>()))
                .Returns(Task.CompletedTask); 

            // act
            var result = await _handler.Handle(command, CancellationToken.None);
            
            // assert
            // 2. Fix: Use ErrorOr properties (IsError) instead of legacy payload mapping properties
            Assert.False(result.IsError); 
            
            // 3. Fix: Typo in assertion message fixed ("successfullt" -> "successfully")
            // Ensure this string perfectly matches the success message inside your actual Handler.
            Assert.Equal("Balance deducted successfully", result.Value.MSG);

            // 4. Verify mock expectations
            _mockRepo.Verify(r => r.ExecuteAsync(
                It.Is<string>(s => s == "sp_deductbalance" || s.Contains("deductbalance")), 
                It.IsAny<object>(),
                It.IsAny<CommandType>()),
                Times.Once
            );
        }
    }
}
