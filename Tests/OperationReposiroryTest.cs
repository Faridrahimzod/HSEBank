using Models;
using Repositories;

using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class OperationRepositoryTests
{
    [Fact]
    public void AddOperation_ShouldStoreOperation()
    {
        var repo = new OperationRepository();
        var operation = new Operation(
            TransactionType.Income,
            Guid.NewGuid(),
            1000m,
            DateTime.Now,
            Guid.NewGuid()
        );

        repo.Add(operation);
        var result = repo.GetById(operation.Id);

        Assert.Equal(operation.Id, result.Id);
    }

    [Fact]
    public void DeleteOperation_ShouldRemoveFromRepository()
    {
        var repo = new OperationRepository();
        var operation = new Operation(TransactionType.Income ,
            new Guid(),
            100000000,
            DateTime.Now,
            new Guid(),
            "");
        repo.Add(operation);

        repo.Delete(operation.Id);
        var result = repo.GetById(operation.Id);

        Assert.Null(result);
    }
}