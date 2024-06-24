using Microsoft.EntityFrameworkCore;
using WalksNZAPI.Data;
using WalksNZAPI.Models.Domain;

namespace WalksNZAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;
        public RegionRepository(NZWalksDbContext _context)
        {
            this._context = _context;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }
    }
}
