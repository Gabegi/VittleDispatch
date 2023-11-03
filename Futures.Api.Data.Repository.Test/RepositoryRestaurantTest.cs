using Futures.Api.Data.Models;
using Futures.Api.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Futures.Api.Data.Repository.Test
{
    public class RepositoryRestaurantTest
    {

   

        // Abstract Class Sqlite
        public abstract class InMemoryTestSetUp : IDisposable
        {
            public const string InMemoryConnectionString = "DataSource=:memory:";
            public readonly SqliteConnection _connection;
            public readonly FuturesContext _context;
            public RestaurantRepository _repository;
            private ILogger<FuturesContext> _logger;

            public InMemoryTestSetUp()
            {
                _connection = new SqliteConnection(InMemoryConnectionString);
                _connection.Open();
                var options = new DbContextOptionsBuilder<FuturesContext>()
                    .UseSqlite(_connection)
                    .Options;
                _context = new FuturesContext(options, _logger);
                _context.Database.EnsureCreated();
                _repository = new RestaurantRepository(_context);

                // Adding Mock data to the in memory database
                var restaurant1 = new Restaurant()
                {

                    RestaurantName = "Royal Motimahal",
                    RestaurantAddress = "Amsterdam",
                    CategoryName = "Indian",
                    ZoneId = 2
                };

                var restaurant2 = new Restaurant()
                {

                    RestaurantName = "test",
                    RestaurantAddress = "Amsterdam",
                    CategoryName = "Indian",
                    ZoneId = 2
                };

                var restaurant3 = new Restaurant()
                {

                    RestaurantName = "",
                    RestaurantAddress = "",
                    CategoryName = "",
                    ZoneId = 3
                };

                async Task AddMockDataToSqlite(Restaurant restaurant)
                {
                    await _repository.Insert(restaurant);
                    _context.SaveChanges();
                }

                AddMockDataToSqlite(restaurant1).GetAwaiter().GetResult();
                AddMockDataToSqlite(restaurant2).GetAwaiter().GetResult();
                AddMockDataToSqlite(restaurant3).GetAwaiter().GetResult();
            }

            public void Dispose() => _connection.Dispose();
        }


        // Testing the db connection setup

        public class InMemoryDataBaseConnectionTest : InMemoryTestSetUp
        {
            [Fact]
            public async Task DatabaseIsAvailableAndCanBeConnectedTo()
            {
                Assert.True(await _context.Database.CanConnectAsync());
            }
        }

        public class AllServicesTests : InMemoryTestSetUp
        {
            [Fact]
            public async Task GetRestaurantByName()
            {
                var restaurant = await _repository.GetByName("Royal Motimahal");

                Assert.Equal("Royal Motimahal", restaurant.RestaurantName);
            }
        }
    }
}