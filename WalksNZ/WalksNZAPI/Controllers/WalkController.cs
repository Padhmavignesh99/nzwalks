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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalkController(IWalkRepository walkRepository,IMapper mapper,
            IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
        public async Task<IActionResult> AddWalkAsync(AddWalkRequest item)
        {
            if(!(await ValidateAddWalksAsync(item)))
            {
                return BadRequest(ModelState);
            }
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
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest item)
        {
            if (!(await ValidateUpdateWalksAsync(item)))
            {
                return BadRequest(ModelState);
            }
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
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDTO>(walk);

            return Ok(walkDto);
        }

        #region Private methods
        private async Task<bool> ValidateAddWalksAsync(AddWalkRequest addwalkRequest)
        {
            if(addwalkRequest == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest),
                    $"{nameof(addwalkRequest)} is Required.");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addwalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addwalkRequest.Name),
                    $"{nameof(addwalkRequest.Name)} is required.");
            }
            if (addwalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addwalkRequest.Length),
                    $"{nameof(addwalkRequest.Length)} should be greater than zero.");
            }
            var region = await regionRepository.GetAsync(addwalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest.RegionId),
                    $"{nameof(addwalkRequest.RegionId)} is Invalid.");
            }
            var walkdifficulty = await walkDifficultyRepository.GetAsync(addwalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest.WalkDifficultyId),
                    $"{nameof(addwalkRequest.WalkDifficultyId)} is Invalid.");
            }
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> ValidateUpdateWalksAsync(UpdateWalkRequest updatewalkRequest)
        {
            if (updatewalkRequest == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest),
                    $"{nameof(updatewalkRequest)} is Required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updatewalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updatewalkRequest.Name),
                    $"{nameof(updatewalkRequest.Name)} is required.");
            }
            if (updatewalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.Length),
                    $"{nameof(updatewalkRequest.Length)} should be greater than zero.");
            }
            var region = await regionRepository.GetAsync(updatewalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.RegionId),
                    $"{nameof(updatewalkRequest.RegionId)} is Invalid.");
            }
            var walkdifficulty = await walkDifficultyRepository.GetAsync(updatewalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.WalkDifficultyId),
                    $"{nameof(updatewalkRequest.WalkDifficultyId)} is Invalid.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
