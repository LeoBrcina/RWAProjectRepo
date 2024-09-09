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
    public class GameGenresController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        public GameGenresController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllGameGenres")]
        public ActionResult<IEnumerable<GameGenreDto>> GetAllGameGenres()
        {
            try
            {
                var result = _context.GameGenres;
                var mappedResult = result.Select(x =>
                    new GameGenreDto
                    {
                        IdgameGenre = x.IdgameGenre,
                        GameId = x.GameId,
                        GenreId = x.GenreId
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
