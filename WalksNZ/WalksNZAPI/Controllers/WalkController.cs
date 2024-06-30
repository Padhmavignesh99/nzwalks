using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalksNZAPI.Data;
using WalksNZAPI.Models.Domain;
using WalksNZAPI.Models.DTO;
using WalksNZAPI.Repositories;

namespace WalksNZAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walks = await walkRepository.GetAllAsync();
            var walkDto = mapper.Map<List<WalkDTO>>(walks);
            return Ok(walkDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync([FromRoute] Guid id)
        {
            var walks = await walkRepository.GetAsync(id);
            if (walks == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDTO>(walks);
            return Ok(walkDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddWalkRequest item)
        {
            var walk = new Walk()
            {
                Name = item.Name,
                Length = item.Length,
                RegionId = item.RegionId,
                WalkDifficultyId = item.WalkDifficultyId
            };

            walk = await walkRepository.AddAsync(walk);

            var walkDto = new WalkDTO()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId
            };
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDto.Id }, walkDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest item)
        {
            var walk = new Walk()
            {
                Name = item.Name,
                Length = item.Length,
                RegionId = item.RegionId,
                WalkDifficultyId = item.WalkDifficultyId
            };

            walk = await walkRepository.UpdateAsync(id, walk);

            if (walk == null)
            {
                return NotFound();
            }
            var walkDto = new WalkDTO()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId
            };
            return Ok(walkDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDTO>(walk);

            return Ok(walkDto);
        }
    }
}
