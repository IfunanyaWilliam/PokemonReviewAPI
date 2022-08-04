using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories =  _categoryRepository.GetAllCategories();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categoryDto);
        }

        [HttpGet("categoryId")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var categoryExist = _categoryRepository.CategoryExists(categoryId);
            if (!categoryExist)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategory(categoryId);
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categoryDto);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategoryId(int categoryId)
        {
            var pokemon = await _categoryRepository.GetPokemonByCategory(categoryId);
            var pokemonDto = _mapper.Map<ICollection<PokemonDTO>>(pokemon);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemonDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if(categoryDto == null)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetAllCategories()
                                              .Where(c => c.Name.Trim().ToUpper() == categoryDto.Name.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryDto);
            var createCategory = await _categoryRepository.CreateCategory(categoryMap);
            if (!createCategory)
            {
                ModelState.AddModelError("", "Something went wrong while creating the Category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }
        
    }
}
