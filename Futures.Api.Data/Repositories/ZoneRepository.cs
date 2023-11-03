using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Futures.Api.Data.Repositories
{
    public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
    {

        public ZoneRepository(IFuturesContext context) : base(context)
        {
        }

        public async Task<Zone?> GetByZoneDescription(string zoneDescription)
        {
            return await _context.Zones
                .Include(zoneRecord => zoneRecord.Riders)
                .SingleOrDefaultAsync(record => record.ZoneDescription != null && record.ZoneDescription.Equals(zoneDescription));
        }

        public Task<IEnumerable<Zone>> GetPaged(int pageSize, int pageMr)
        {
            throw new NotImplementedException();
        }
    }
}
