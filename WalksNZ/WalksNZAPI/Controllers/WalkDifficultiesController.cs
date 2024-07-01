using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalksNZAPI.Models.Domain;
using WalksNZAPI.Models.DTO;
using WalksNZAPI.Repositories;

namespace WalksNZAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetallWalk()
        {
            var walkdif = await walkDifficultyRepository.GetAllAsync();

            if (walkdif == null)
            {
                return NotFound();
            }
            var walkdifDto = mapper.Map<List<WalkDifficultyDTO>>(walkdif);

            return Ok(walkdifDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalk")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walkdif= await walkDifficultyRepository.GetAsync(id);

            if (walkdif == null)
            {
                return NotFound();
            }
            var walkdifDto = mapper.Map<WalkDifficultyDTO>(walkdif);

            return Ok(walkdifDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalk(AddWalkDifficultyRequest item)
        {
            if (!ValidateAddWalkdifficultyAsync(item))
            {
                return BadRequest(ModelState);
            }
            var walkDifficulty = new WalkDifficulty
            {
                Code = item.Code,
            };


            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            var walkdifDto = mapper.Map<WalkDifficultyDTO>(walkDifficulty);

            return CreatedAtAction(nameof(GetWalk),new { id = walkdifDto.Id }, walkdifDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest item)
        {
            if (!ValidateUpdateWalkdifficultyAsync(item))
            {
                return BadRequest(ModelState);
            }
            var walk = new WalkDifficulty
            {
                Code = item.Code
            };

            walk = await walkDifficultyRepository.UpdateAsync(id, walk);

            if (walk == null)
            {
                return NotFound();
            }
            var walkdifDto = mapper.Map<WalkDifficultyDTO>(walk);

            return Ok(walkdifDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {

            var walk = await walkDifficultyRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }
            var walkdifDto = mapper.Map<WalkDifficultyDTO>(walk);

            return Ok(walkdifDto);
        }

        #region Private methods
        private bool ValidateAddWalkdifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                    $"{nameof(addWalkDifficultyRequest)} is Required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} is Required.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkdifficultyAsync(UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"{nameof(updateWalkDifficultyRequest)} is Required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} is Required.");
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
