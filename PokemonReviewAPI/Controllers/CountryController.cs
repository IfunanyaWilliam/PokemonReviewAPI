using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepo = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepo.GetAllCountries();
            var countryDto = _mapper.Map<List<CountryDTO>>(countries);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countryDto);
        }


        [HttpGet("countryId")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            var countryExists = await _countryRepo.CountryExistsAsync(countryId);
            if(!countryExists)
                return NotFound();

            var country = await _countryRepo.GetCountryAsync(countryId);
            var countryDto = _mapper.Map<CountryDTO>(country);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countryDto);
        }


        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryOfOwner(int ownerId)
        {
            var country = await _countryRepo.GetCountryByOwnerAsync(ownerId);
            var countryDto = _mapper.Map<CountryDTO>(country);

            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(countryDto);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDTO countryDto)
        {
            if(countryDto == null)
                return BadRequest(ModelState);

            var category = _countryRepo.GetAllCountries()
                                              .Where(c => c.Name.Trim().ToUpper() == countryDto.Name.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if(category != null)
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryDto);
            var createCategory = await _countryRepo.CreateCountryAsync(countryMap);
            if(!createCategory)
                ModelState.AddModelError("", "Something went wrong while creating Country");
                return StatusCode(500, ModelState);

            return Ok("Country Successfully Created");
        }


        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryDTO countryDto)
        {
            if(countryDto == null)
                return BadRequest(ModelState);

            if(countryId != countryDto.Id)
                return BadRequest(ModelState);

            var country = await _countryRepo.CountryExistsAsync(countryId);
            if(!country)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(countryDto);
            var updateCategory = await _countryRepo.UpdateCountryAsync(countryMap);
            if(!updateCategory)
                ModelState.AddModelError("", "Category could not be upddated");
                return StatusCode(500, ModelState);

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            var countryExists = await _countryRepo.CountryExistsAsync(countryId);
            if(!countryExists)
                return NotFound();

            var countryToDelete = await _countryRepo.GetCountryAsync(countryId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _countryRepo.DeleteCountryAsync(countryToDelete);
            if(!result)
                ModelState.AddModelError("", "Something went wrong deleting country");

            return NoContent();
        }
    }
}
