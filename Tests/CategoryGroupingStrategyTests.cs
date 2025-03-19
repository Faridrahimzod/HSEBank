using Analytics;
using Interfaces;
using Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class CategoryGroupingStrategyTests
{
    [Fact]
    public void Group_ShouldSumAmountsByCategory()
    {
        var strategy = new CategoryGroupingStrategy();
        var categoryId = Guid.NewGuid();

        var operations = new[]
        {
            new Operation(TransactionType.Expense, Guid.NewGuid(), 500m, DateTime.Now, categoryId),
            new Operation(TransactionType.Expense, Guid.NewGuid(), 300m, DateTime.Now, categoryId)
        };

        var categories = new List<Category>
        {
            new Category(TransactionType.Expense, "Food") { Id = categoryId }
        };

        var mockCategoryRepo = new Mock<IRepository<Category>>();
        mockCategoryRepo.Setup(repo => repo.GetAll()).Returns(categories);

        var result = strategy.Group(operations, mockCategoryRepo.Object);

        Assert.Equal(800m, result.First().Value); 
    }
}