using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(NZWalksDbContext dbContext, IWalkRepository walkRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        // CREATE Walk
        // POST:

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
           
                // Map DTO to Domain Model

                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                // Use Domain Model to Create Walk

                walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

                // Map Domain Model back to DTO

                var walkDto = mapper.Map<WalkDto>(walkDomainModel);

                return Ok();
           

        }



        // Get Walks 

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            // Get Data from Database - Domain Models

            var walksDomainModel = await walkRepository.GetAllAsync();

            // Return DTOs And Map Domain Model to DTO

            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }



        // Get Walks By ID

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var walk = dbcontext.walks.find(id);

            // Get Walk Domain Model from Database

            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Return Domain  Model to DTO

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        // Update Walks By Id

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]

        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
           
                // Map DTO to Domain Model

                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                // Check if Walk exists

                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                // Convert Domain Model to DTO

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
           
        }


        // Delete Walk

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }


            // Map Domain Model to DTO

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
