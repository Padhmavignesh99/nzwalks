using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WalksNZAPI.Models.Domain;
using WalksNZAPI.Models.DTO;
using WalksNZAPI.Repositories;

namespace WalksNZAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();
            var regionDto = mapper.Map<List<RegionDTO>>(regions);
            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync([FromRoute]Guid id)
        {
            var regions = await regionRepository.GetAsync(id);
            if(regions == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDTO>(regions);
            return Ok(regionDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest item)
        {
            var region = new Region()
            {
                Name = item.Name,
                Code = item.Code,
                Area = item.Area,
                Lat = item.Lat,
                Long = item.Long,
                Population = item.Population
            };

            region = await regionRepository.AddAsync(region);

            var regionDto = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDto.Id },regionDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if(region == null) 
            { 
                return NotFound();
            }

            var regionDto = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return Ok(regionDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody] UpdateRegionRequest item)
        {
            var region = new Region()
            {
                Name = item.Name,
                Code = item.Code,
                Area = item.Area,
                Lat = item.Lat,
                Long = item.Long,
                Population = item.Population
            };

            region = await regionRepository.UpdateAsync(id, region);
            if(region == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return Ok(regionDto);
        }
    }
}
