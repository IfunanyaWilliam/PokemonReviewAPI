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
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetAllCountries();
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
        public async Task<IActionResult> GetCountry(int countryId)
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


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDTO countryDto)
        {
            if (countryDto == null)
            {
                return BadRequest(ModelState);
            }
            var category = _countryRepository.GetAllCountries()
                                              .Where(c => c.Name.Trim().ToUpper() == countryDto.Name.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryDto);
            var createCategory = await _countryRepository.CreateCountry(countryMap);
            if (!createCategory)
            {
                ModelState.AddModelError("", "Something went wrong while creating Country");
                return StatusCode(500, ModelState);
            }

            return Ok("Country Successfully Created");
        }

    }
}
