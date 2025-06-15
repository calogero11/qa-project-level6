using Microsoft.EntityFrameworkCore;
using webapi.Data;

namespace webapi.UnitTests.Helpers;

public static class TestDbContextFactory
{
    public static DatabaseContext Create(string dbName)
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        
        return new DatabaseContext(options);
    }
    
}