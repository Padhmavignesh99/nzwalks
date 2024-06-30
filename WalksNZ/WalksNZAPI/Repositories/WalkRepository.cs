using Microsoft.EntityFrameworkCore;
using WalksNZAPI.Data;
using WalksNZAPI.Models.Domain;

namespace WalksNZAPI.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _context;
        public WalkRepository(NZWalksDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }
            _context.Walks.Remove(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _context.Walks.Include(x=>x.Region).Include(x=>x.walkDifficulty).ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await _context.Walks.Include(x => x.Region).Include(x => x.walkDifficulty).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _context.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Length = walk.Length;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await _context.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
    }
}
