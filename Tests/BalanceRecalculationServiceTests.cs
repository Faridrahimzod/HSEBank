using Analytics;
using Repositories;
using Interfaces;
using Models;
using Moq;
using Xunit;

public class BalanceRecalculationServiceTests
{
    [Fact]
    public void RecalculateBalance_ShouldUpdateAccountCorrectly()
    {
        var accountId = Guid.NewGuid();
        var mockAccountRepo = new Mock<IRepository<BankAccount>>();
        var mockOperationRepo = new Mock<IRepository<Operation>>();

        mockOperationRepo.Setup(repo => repo.GetAll())
            .Returns(new[]
            {
                new Operation(TransactionType.Income, accountId, 1000m, DateTime.Now, Guid.NewGuid()),
                new Operation(TransactionType.Expense, accountId, 300m, DateTime.Now, Guid.NewGuid())
            });

        var service = new BalanceRecalculationService(
            mockAccountRepo.Object,
            mockOperationRepo.Object
        );

        service.RecalculateBalance(accountId);

        mockAccountRepo.Verify(repo => repo.Update(It.Is<BankAccount>(a => a.Balance == 700m)));
    }
}