using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepo,
                               ICountryRepository countryRepo,
                                IMapper mapper)
        {
            _ownerRepo = ownerRepo;
            _countryRepo = countryRepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owner = _ownerRepo.GetAllOwners();
            var ownerDto = _mapper.Map<List<OwnerDTO>>(owner);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(ownerDto);
        }

        [HttpGet("ownerId")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            var ownerExist = await _ownerRepo.OwnerExists(ownerId);
            if (!ownerExist)
            {
                return NotFound();
            }

            var owner = await _ownerRepo.GetOwner(ownerId);
            var ownerDto = _mapper.Map<OwnerDTO>(owner);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(ownerDto);
        }


        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonByOwner(int ownerId)
        {
            var ownerExists = await _ownerRepo.OwnerExists(ownerId);
            if (!ownerExists)
            {
                return NotFound();
            }

            var pokemons        = _ownerRepo.GetPokemonByOwner(ownerId);
            var ownerPokemons   = _mapper.Map<ICollection<PokemonDTO>>(pokemons);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(ownerPokemons);

        }



        [HttpPost]
        [ProducesResponseType(20)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO ownerDto)
        {
            if (ownerDto == null)
            {
                return BadRequest(ModelState);
            }
            var owners = _ownerRepo.GetAllOwners()
                                              .Where(c => c.LastName.Trim().ToUpper() == ownerDto.LastName.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerDto);

            //Insert the country of the owner
            ownerMap.Country = await _countryRepo.GetCountryAsync(countryId);

            var createCategory = await _ownerRepo.CreateOwner(ownerMap);
            if (!createCategory)
            {
                ModelState.AddModelError("", "Something went wrong while creating the Owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Owner Successfully Created");
        }
    }
}
