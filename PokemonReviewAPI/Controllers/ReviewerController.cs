using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepo, IMapper mapper)
        {
            _reviewerRepo = reviewerRepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviews()
        {
            var reviewers = _reviewerRepo.GetAllReviewers();
            var reviewerDto = _mapper.Map<List<ReviewerDTO>>(reviewers);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewerDto);
        }


        [HttpGet("reviewerId")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            var reviewerExist = await _reviewerRepo.ReviewerExists(reviewerId);
            if (!reviewerExist)
            {
                return NotFound();
            }

            var reviewer = _reviewerRepo.GetReviewer(reviewerId);
            var reviewerDto = _mapper.Map<ReviewerDTO>(reviewer);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}/reviews")]
        //[ProducesResponseType(200, Type = typeof(Reviewer))]
        public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId)
        {
            var reviewerExist = await _reviewerRepo.ReviewerExists(reviewerId);
            if (!reviewerExist)
            {
                return NotFound();
            }

            var reviews = _reviewerRepo.GetReviewsByReviewer(reviewerId);
            var reviewersDto = _mapper.Map<ICollection<ReviewDTO>>(reviews);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewersDto);
        }


    }
}
