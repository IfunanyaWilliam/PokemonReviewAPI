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
        public async Task<IActionResult> GetPokemons()
        {
            var pokemon = await _pokemonRepository.GetAllPokemonsAsync();
            var pokemonDto = _mapper.Map<List<PokemonDTO>>(pokemon);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemonDto);
        }

        [HttpGet("pokeId")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokeId)
        {
            var pokemonExist = _pokemonRepository.PokemonExist(pokeId);
            if (!pokemonExist)
            {
                return NotFound();
            }

            var pokemon = await _pokemonRepository.GetPokemonAsync(p => p.Id == pokeId);
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
        public IActionResult GetPokemonRating(int pokeId)
        {
            var pokemonExist =  _pokemonRepository.PokemonExist(pokeId);
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
    }
}
