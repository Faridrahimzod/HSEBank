using Models;
using ImportExport;
using Xunit;

public class JsonImporterTests
{
    [Fact]
    public void Import_ShouldRestoreAllEntities()
    {
        var importer = new JsonImporter();
        var testFile = "test_data.json";

        var (accounts, categories, operations) = importer.Import(testFile);

        Assert.Equal(2, accounts.Count);
        Assert.Equal(3, categories.Count);
        Assert.Equal(5, operations.Count);
    }
}