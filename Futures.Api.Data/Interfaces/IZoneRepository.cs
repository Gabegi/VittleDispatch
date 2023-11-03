using Futures.Api.Data.Models;

namespace Futures.Api.Data.Interfaces
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        Task<Zone?> GetByZoneDescription(string zoneDescription);


        Task<IEnumerable<Zone>> GetPaged(int pageSize, int pageMr);
    }
}
