using Microsoft.EntityFrameworkCore;
using WalksNZAPI.Data;
using WalksNZAPI.Models.Domain;

namespace WalksNZAPI.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _context;

        public WalkDifficultyRepository(NZWalksDbContext _context)
        {
            this._context = _context;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _context.WalkDifficulty.AddAsync(walkDifficulty);
            await _context.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWd = await _context.WalkDifficulty.FindAsync(id);
            if (existingWd == null)
            {
                return null;
            }

            _context.WalkDifficulty.Remove(existingWd);
            await _context.SaveChangesAsync();
            return existingWd;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _context.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _context.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWd = await _context.WalkDifficulty.FindAsync(id);
            if (existingWd == null)
            {
                return null;
            }

            existingWd.Code = walkDifficulty.Code;
            await _context.SaveChangesAsync();
            return existingWd;
        }
    }
}
