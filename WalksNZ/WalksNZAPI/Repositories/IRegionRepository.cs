using WalksNZAPI.Models.Domain;

namespace WalksNZAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
