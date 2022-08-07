using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository,
                                 IReviewRepository reviewRepo,
                                 IMapper mapper)
        {
            _pokemonRepo = pokemonRepository;
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemon = _pokemonRepo.GetAllPokemons();
            var pokemonDto = _mapper.Map<List<PokemonDTO>>(pokemon);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonDto);
        }

        [HttpGet("pokemonId")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokemonId)
        {
            var pokemonExist = await _pokemonRepo.PokemonExistsAsync(pokemonId);
            if(!pokemonExist)
                return NotFound();

            var pokemon = await _pokemonRepo.GetPokemonAsync(p => p.Id == pokemonId);
            var pokemonDto = _mapper.Map<PokemonDTO>(pokemon);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonDto);
        }


        [HttpGet("pokeId/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRating(int pokeId)
        {
            var pokemonExist = await _pokemonRepo.PokemonExistsAsync(pokeId);
            if(!pokemonExist)
                return NotFound();

            var pokemonRating = _pokemonRepo.GetPokemoneRating(pokeId);
            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemonRating);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, int categoryId, [FromBody] PokemonDTO pokemonDto)
        {
            if(pokemonDto == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepo.GetAllPokemons()
                                              .Where(c => c.Name.Trim().ToUpper() == pokemonDto.Name.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if(pokemons != null)
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
            var createPokemon = await _pokemonRepo.CreatePokemonAsync(ownerId, categoryId, pokemonMap);

            if(!createPokemon)
                ModelState.AddModelError("", "Something went wrong while creating the Pokemon");
                return StatusCode(500, ModelState);

            return Ok("Successfully Created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, int pokemonId, [FromBody] PokemonDTO pokemonDto)
        {
            if(pokemonDto == null)
                return BadRequest(ModelState);

            if(pokemonId != pokemonDto.Id)
                return BadRequest(ModelState);

            var pokemon = await _pokemonRepo.PokemonExistsAsync(pokemonId);

            if(!pokemon)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
            var updatePokemon = await _pokemonRepo.UpdatePokemonAsync(ownerId, categoryId, pokemonMap);

            if(!updatePokemon)
                ModelState.AddModelError("", "Pokemon could not be upddated");
                return StatusCode(500, ModelState);

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokemonId)
        {
            var pokemon = await _pokemonRepo.PokemonExistsAsync(pokemonId);
            if(!pokemon)
                return NotFound();

             
            var reviewsToDelete = _reviewRepo.GetReviewsOfAPokemon(pokemonId);
            var pokemonToDelete = await _pokemonRepo.GetPokemonAsync(p => p.Id == pokemonId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //Delete reviews associated with a pokemon
            var deleteReviews = await _reviewRepo.DeleteReviewsAsync(reviewsToDelete.ToList());
            if(!deleteReviews)
                ModelState.AddModelError("", "Something went wrong while deleting reviews");

            var deletePokemon = await _pokemonRepo.DeletePokemonAsync(pokemonToDelete);
            if(!deletePokemon)
                ModelState.AddModelError("", "Something went wrong deleting pokemon");

            return NoContent();
        }
    }
}
