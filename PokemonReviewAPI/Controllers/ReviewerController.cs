namespace PokemonReviewAPI.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Contract;
    using DTO;
    using Models;


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewerDto);
        }


        [HttpGet("reviewerId")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            var reviewerExist = await _reviewerRepo.ReviewerExistsAsync(reviewerId);

            if(!reviewerExist)
                return NotFound();

            var reviewer = _reviewerRepo.GetReviewer(reviewerId);
            var reviewerDto = _mapper.Map<ReviewerDTO>(reviewer);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}/reviews")]
        //[ProducesResponseType(200, Type = typeof(Reviewer))]
        public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId)
        {
            var reviewerExist = await _reviewerRepo.ReviewerExistsAsync(reviewerId);

            if(!reviewerExist)
                return NotFound();

            var reviews = _reviewerRepo.GetReviewsByReviewer(reviewerId);
            var reviewersDto = _mapper.Map<ICollection<ReviewDTO>>(reviews);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewersDto);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDTO reviewerDto)
        {
            if(reviewerDto == null)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepo.GetAllReviewers()
                                              .Where(c => c.LastName.Trim().ToUpper() == reviewerDto.LastName.TrimEnd().ToUpper())
                                              .FirstOrDefault();

            if(reviewer != null)
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);
            var createReviewer = await _reviewerRepo.CreateReviewerAsync(reviewerMap);

            if(!createReviewer)
                ModelState.AddModelError("", "Something went wrong while creating the Reviewer");
                return StatusCode(500, ModelState);

            return Ok("Reviewer Successfully Created");
        }


        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerDto)
        {
            if(reviewerDto == null)
                return BadRequest(ModelState);

            if(reviewerId != reviewerDto.Id)
                return BadRequest(ModelState);

            var reviewer = await _reviewerRepo.ReviewerExistsAsync(reviewerId);
            if(!reviewer)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);
            var updateReviewer = await _reviewerRepo.UpdateReviewerAsync(reviewerMap);

            if(!updateReviewer)
                ModelState.AddModelError("", "Reviewer could not be upddated");
                return StatusCode(500, ModelState);

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            var reviewExists = await _reviewerRepo.ReviewerExistsAsync(reviewerId);
            if(!reviewExists)
                return NotFound();

            var reviewToDelete = _reviewerRepo.GetReviewer(reviewerId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleteReview = await _reviewerRepo.DeleteReviewerAsync(reviewToDelete);

            if(!deleteReview)
                ModelState.AddModelError("", "Something went wrong deleting reviewer");

            return NoContent();
        }
    }
}
