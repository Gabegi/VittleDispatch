using Futures.Api.Data.Models;
using Futures.Api.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Futures.Api.Data.Repository.Test
{
    public class RepositoryZoneTest
    {
        public abstract class InMemoryTestSetUp : IDisposable
        {
            public const string InMemoryConnectionString = "DataSource=:memory:";
            public readonly SqliteConnection _connection;
            public readonly FuturesContext _context;
            public ZoneRepository _repository;
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
                _repository = new ZoneRepository(_context);

                // Adding Mock data to the in memory database
                var zone1 = new Zone()
                {
                    ZoneId = 1,
                    ZoneDescription = "Italy"
                };

                var zone2 = new Zone()
                {
                    ZoneId = 2,
                    ZoneDescription = "US"
                };

                var zone3 = new Zone()
                {
                    ZoneId = 3,
                    ZoneDescription = "Argentina"
                };

                var zone4 = new Zone()
                {
                    ZoneId = 4,
                    ZoneDescription = ""
                };

                _repository.Insert(zone1).GetAwaiter().GetResult();
                _repository.Insert(zone2).GetAwaiter().GetResult();
                _repository.Insert(zone3).GetAwaiter().GetResult();
                _repository.Insert(zone4).GetAwaiter().GetResult();
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

        // Doing the tests
        public class AllServicesTests : InMemoryTestSetUp
        {
            public Zone checkedZone;

            [Fact]
            public async Task GetZoneDescription_InputZone1_ReturnsEqual()
            {
                var zone = await _repository.GetByZoneDescription("Italy");
                Assert.Equal("Italy", zone?.ZoneDescription);
            }


            [Theory]
            [InlineData("Italy")]
            [InlineData("")]
            [InlineData("US")]
            [InlineData("Argentina")]
            public async Task GetZoneDescription_InputZones_ReturnsEqual(string inputToCompare)
            {
                var zone = await _repository.GetByZoneDescription(inputToCompare);
                Assert.Equal(inputToCompare, zone?.ZoneDescription);
            }


            [Fact]
            public async Task GetZoneDescription_InputZone1_ReturnsNull()
            {
                var zone = await _repository.GetByZoneDescription("Australia");
                Assert.Null(zone);
            }


            [Fact]
            public async Task GetAll_ReturnsEqual()
            {
                var zones = await _repository.GetAll();

                checkedZone = zones.First();

                Assert.Equal(4, zones.Count());

                Assert.IsType<Zone>(checkedZone);

                Assert.Equal("Italy", checkedZone.ZoneDescription);
            }


            [Fact]
            public async Task GetById_ReturnsEqual()
            {
                var zone = await _repository.GetById(1);
                var checking = await _repository.GetByZoneDescription("Italy");

                var notexistingId = await _repository.GetById(258455);
                var negativeId = await _repository.GetById(-1);

                Assert.Equal(checking, zone);
                Assert.Equal("Italy", zone?.ZoneDescription);
                Assert.Null(notexistingId);
                Assert.Null(negativeId);
            }


            [Fact]
            public async Task Insert_NewZone_ReturnsEqual()
            {
                var zone5 = new Zone()
                {
                    ZoneId = 5,
                    ZoneDescription = "Mexico"
                };

                await _repository.Insert(zone5);
                var zone = await _repository.GetByZoneDescription("Mexico");

                Assert.Equal("Mexico", zone?.ZoneDescription);
            }

            [Fact]
            public async Task Upsert_ReturnsEqual()
            {
                var zone5 = new Zone()
                {
                    ZoneId = 4,
                    ZoneDescription = "Netherlands"
                };

                var result = await _repository.GetById(zone5.ZoneId);

                if (result != null)
                {
                    result.ZoneDescription = zone5.ZoneDescription;

                    await _repository.Update(result);
                }

                var zone = await _repository.GetByZoneDescription("Netherlands");

                Assert.Equal(4, zone?.ZoneId);
                Assert.IsType<Zone>(zone);
            }

            [Fact]
            public async Task Delete_Zone_ReturnsEqual()
            {
                var zone5 = new Zone()
                {
                    ZoneId = 256,
                    ZoneDescription = "Netherlands"
                };

                await _repository.Insert(zone5);

                await _repository.DeleteAsync(256);

                var zone = await _repository.GetById(256);

                Assert.Null(zone);
            }

        }
    }
}