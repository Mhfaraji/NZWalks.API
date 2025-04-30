
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext , IRegionRepository regionRepository , IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()   
        {
            // Get data from database - domain models

            var regionsDomain = await regionRepository.GetAllAsync();

            // Return DTOs

            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }



        // Get Region By Id (single region):
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
           // var region = dbContext.Regions.Find(id);

           // Get region domain model from database

           var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
           
            // return dto back to client

            return Ok(mapper.Map<List<RegionDto>>(regionDomain));

        }


        //Post to Create new region 
        

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                // Map or convert dto to domain model

                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);



                // Use Domain Model to Create Region

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Map Domain Model back to DTO

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
                
        }

        // Update Region

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
            


        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
          
                // Map DTO to Domain Model

                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // check if region exists

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // convert Domain Model to DTO

                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            
           
        }


        // Delete Region

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

          if (regionDomainModel == null)
          {
              return NotFound();
          }

            // return deleted Region Back


            // Map Domain Model to DTO

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

    }
}
