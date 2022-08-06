using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IPokemonRepository _pokemonRepo;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepo,
                                IReviewerRepository reviewerRepo,
                                IPokemonRepository pokemonRepo,
                                IMapper mapper)
        {
            _reviewRepo     = reviewRepo;
            _pokemonRepo    = pokemonRepo;
            _reviewerRepo   = reviewerRepo;
            _mapper         = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var review = _reviewRepo.GetAllReviews();
            var reviewDto = _mapper.Map<List<ReviewDTO>>(review);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewDto);
        }


        [HttpGet("reviewId")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            var reviewExist = await _reviewRepo.ReviewExists(reviewId);
            if (!reviewExist)
            {
                return NotFound();
            }

            var review = _reviewRepo.GetReview(reviewId);
            var reviewDto = _mapper.Map<ReviewDTO>(review);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewDto);
        }


        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokemonId)
        {
            var pokemonReviews = _reviewRepo.GetReviewsOfAPokemon(pokemonId);
            var reviewsDto = _mapper.Map<List<ReviewDTO>>(pokemonReviews);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewsDto);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest(ModelState);
            }
            var reviews = _reviewRepo.GetAllReviews()
                                              .Where(c => c.Text.Trim().ToUpper() == reviewDto.Text.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var reviewMap = _mapper.Map<Review>(reviewDto);
            reviewMap.Pokemon = await _pokemonRepo.GetPokemonAsync(p => p.Id == pokemonId);
            reviewMap.Reviewer = _reviewerRepo.GetReviewer(reviewerId);
            var createReview = await _reviewRepo.CreateReview(reviewMap);
            if (!createReview)
            {
                ModelState.AddModelError("", "Something went wrong while creating the Review");
                return StatusCode(500, ModelState);
            }

            return Ok("Review Successfully Created");
        }
    }
}
