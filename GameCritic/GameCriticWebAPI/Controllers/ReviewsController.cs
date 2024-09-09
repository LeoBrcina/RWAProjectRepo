using GameCritic.DTOModels;
using GameCriticBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        public ReviewsController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllReviews")]
        public ActionResult<IEnumerable<ReviewDto>> GetAllReviews()
        {
            try
            {
                var result = _context.Reviews;
                var mappedResult = result.Select(x =>
                    new ReviewDto
                    {
                        Idreview = x.Idreview,
                        GameId = x.GameId,
                        GamerId = x.GamerId,
                        Rating = x.Rating,
                        ReviewText = x.ReviewText
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ReviewDto> GetReviewById(int id)
        {
            try
            {
                var result =
                    _context.Reviews
                        .FirstOrDefault(x => x.Idreview == id);

                if (result == null)
                {
                    return NotFound($"Could not find review with id {id}");
                }

                var mappedResult = new ReviewDto
                {
                    Idreview = result.Idreview,
                    GameId = result.GameId,
                    GamerId = result.GamerId,
                    Rating = result.Rating,
                    ReviewText = result.ReviewText
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateReview")]
        public ActionResult<ReviewDto> PostReview([FromBody] ReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newReview = new Review
                {
                    Idreview = reviewDto.Idreview,
                    GameId = reviewDto.GameId,
                    GamerId = reviewDto.GamerId,
                    Rating = reviewDto.Rating,
                    ReviewText = reviewDto.ReviewText
                };

                _context.Reviews.Add(newReview);

                _context.SaveChanges();

                reviewDto.Idreview = newReview.Idreview;

                return Ok(reviewDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateReview")]
        public ActionResult<ReviewDto> PutReview(int id, [FromBody] ReviewDto updatedReviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedReview = _context.Reviews.FirstOrDefault(x => x.Idreview == id);
                if (updatedReview == null)
                {
                    return NotFound();
                }

                updatedReview.Rating = updatedReviewDto.Rating;
                updatedReview.ReviewText = updatedReviewDto.ReviewText;

                _context.SaveChanges();

                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteReview")]
        public ActionResult<ReviewDto> DeleteReview(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var reviewToDelete = _context.Reviews.FirstOrDefault(x => x.Idreview == id);
                if (reviewToDelete == null)
                {
                    return NotFound();
                }

                _context.Reviews.Remove(reviewToDelete);

                _context.SaveChanges();

                return Ok(reviewToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
