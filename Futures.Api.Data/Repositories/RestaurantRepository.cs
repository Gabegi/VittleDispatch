using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Futures.Api.Data.Repositories
{
    /// <summary>
    /// The implementation of IRestaurantRepository. This provides all the functionality of the GenericRepository, and adds the GetByName.
    /// </summary>
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(IFuturesContext context) : base(context) // create a fake one for UniTesting > create a fake db represented by context > create a new project
        {
        }
        // Futures.API.Data Futures UnitTesting Project

        /// <summary>
        /// Retrieves a restaurant, with all its attached items
        /// <param name="restaurantName"></param>
        /// </summary>
        /// <returns></returns>
        public async Task<Restaurant> GetByName(string restaurantName)
        {
            return await _context.Restaurants
                .Include(restaurantRecord => restaurantRecord.Dishes)
                .Include(restaurantRecord => restaurantRecord.Zone)
                .Include(restaurantRecord => restaurantRecord.CategoryNameNavigation)
                .SingleOrDefaultAsync(record => record.RestaurantName.ToLower().Contains(restaurantName.ToLower()));
        }
    }
}
