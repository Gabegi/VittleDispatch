using Futures.ApiContracts;

namespace Futures.Services.ServiceInterfaces
{
    public interface IZoneService
    {
        Task<GetZoneResponseModel> GetbyIdAsync(int zoneId);

        Task<IEnumerable<GetZoneResponseModel>> GetAllAsync();

        void DeleteByIdAsync(int zoneId);

        void InsertNewAsync();

        void UpdateAsync(GetZoneResponseModel zone);
    }
}
