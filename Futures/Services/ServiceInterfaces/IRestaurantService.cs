using Futures.ApiContracts;

namespace Futures.Services.ServiceInterfaces
{
    public interface IRestaurantService
    {
        Task<GetRestaurantResponseModel> GetbyIdAsync(int restaurantId);

        Task<GetRestaurantResponseModel> GetbyNameAsync(string restaurantName);

        Task<IEnumerable<GetRestaurantResponseModel>> GetAllAsync();

        Task DeleteByIdAsync(int restaurantId);

        Task InsertNewAsync(GetRestaurantResponseModel newRestaurant);

        Task UpdateAsync(GetRestaurantResponseModel newRestaurant);

    }
}
