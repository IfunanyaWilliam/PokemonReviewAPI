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
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetOwners()
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
            var reviewExist = await _reviewRepo.ReviewExist(reviewId);
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


    }
}
