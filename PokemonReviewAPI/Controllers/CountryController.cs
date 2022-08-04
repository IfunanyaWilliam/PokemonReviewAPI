using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public async Task<IActionResult> GetCategories()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();
            var countryDto = _mapper.Map<List<CountryDTO>>(countries);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countryDto);
        }


        [HttpGet("countryId")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int countryId)
        {
            var countryExists = _countryRepository.CountryExists(countryId);
            if (!countryExists)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetCountryAsync(countryId);
            var countryDto = _mapper.Map<CountryDTO>(country);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countryDto);
        }


        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryOfOwner(int ownerId)
        {
            var country = await _countryRepository.GetCountryByOwnerAsync(ownerId);
            var countryDto = _mapper.Map<CountryDTO>(country);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(countryDto);
        }

    }
}
