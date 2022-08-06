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
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemon = _pokemonRepository.GetAllPokemons();
            var pokemonDto = _mapper.Map<List<PokemonDTO>>(pokemon);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemonDto);
        }

        [HttpGet("pokemonId")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokemonId)
        {
            var pokemonExist = await _pokemonRepository.PokemonExists(pokemonId);
            if (!pokemonExist)
            {
                return NotFound();
            }

            var pokemon = await _pokemonRepository.GetPokemonAsync(p => p.Id == pokemonId);
            var pokemonDto = _mapper.Map<PokemonDTO>(pokemon);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonDto);
        }


        [HttpGet("pokeId/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRating(int pokeId)
        {
            var pokemonExist = await _pokemonRepository.PokemonExists(pokeId);
            if (!pokemonExist)
            {
                return NotFound();
            }

            var pokemonRating = _pokemonRepository.GetPokemoneRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemonRating);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, int categoryId, [FromBody] PokemonDTO pokemonDto)
        {
            if (pokemonDto == null)
            {
                return BadRequest(ModelState);
            }
            var pokemons = _pokemonRepository.GetAllPokemons()
                                              .Where(c => c.Name.Trim().ToUpper() == pokemonDto.Name.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
            var createPokemon = await _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap);
            if (!createPokemon)
            {
                ModelState.AddModelError("", "Something went wrong while creating the Pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }
    }
}
