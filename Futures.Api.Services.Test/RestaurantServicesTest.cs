using Futures.ApiContracts;
using Futures.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Futures.Api.Services.Test
{

    [TestFixture]
    public class RestaurantServicesTest
    {


        public ILogger<RestaurantService> _logger;

        RestaurantService services;

        // Create Mock Logger
        public RestaurantServicesTest()
        {
            _logger = new Mock<ILogger<RestaurantService>>().Object;

             services = new RestaurantService(_logger);
        }

        [Test]
        public void GetByIdAsync_InputWrongId_ReturnsException()
        {
            // Arrange
            int negativeId = -1;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => services.GetbyIdAsync(negativeId));
        }


        [Test]
        public void GetbyNameAsync_InputWrongName_ReturnsException()
        {
            // Arrange
            string emptyString = "";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => services.GetbyNameAsync(emptyString));
        }

        [Test]
        public void DeleteByIdAsync_InputWrongName_ReturnsException()
        {
            // Arrange
            int negativeId = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => services.DeleteByIdAsync(negativeId).GetAwaiter().GetResult());
        }
        
        [Test]
        public void InsertNewAsync_InputWrongName_ReturnsException()
        {
            // Arrange
            GetRestaurantResponseModel emptyRestaurant = new();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => services.InsertNewAsync(emptyRestaurant).GetAwaiter().GetResult());
        }
        
        [Test]
        public void UpdateAsync_InputWrongName_ReturnsException()
        {
            // Arrange
            GetRestaurantResponseModel emptyRestaurant = new();


            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => services.UpdateAsync(emptyRestaurant).GetAwaiter().GetResult());
        }
    }
}