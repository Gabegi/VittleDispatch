using Futures.Api.Data.Models;


namespace Futures.Api.Data.Interfaces
{
    /// <summary>
    /// Speficic interface for the restaurant entity. Adds a GetByName method to the methods already provided by the generic repository.
    /// </summary>
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<Restaurant> GetByName(string restaurantName);
        
    }
}
