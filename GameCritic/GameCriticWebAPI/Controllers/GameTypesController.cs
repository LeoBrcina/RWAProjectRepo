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
    public class GameTypesController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        public GameTypesController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllGameTypes")]
        public ActionResult<IEnumerable<GameTypeDto>> GetAllGamesTypes()
        {
            try
            {
                var result = _context.GameTypes;
                var mappedResult = result.Select(x =>
                    new GameTypeDto
                    {
                        IdGameType = x.IdgameType,
                        GameTypeName = x.GameTypeName,
                        Description = x.Description,
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GameDto> GetGameTypeById(int id)
        {
            try
            {
                var result =
                    _context.GameTypes
                        .FirstOrDefault(x => x.IdgameType == id);

                if (result == null)
                {
                    return NotFound($"Could not find game type with id {id}");
                }

                var mappedResult = new GameTypeDto
                {
                    IdGameType = result.IdgameType,
                    GameTypeName = result.GameTypeName,
                    Description = result.Description,
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateGameType")]
        public ActionResult<GameTypeDto> PostGameType([FromBody] GameTypeDto gameTypeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newGameType = new GameType
                {
                    GameTypeName = gameTypeDto.GameTypeName,
                    Description = gameTypeDto.Description,
                };

                _context.GameTypes.Add(newGameType);

                _context.SaveChanges();

                gameTypeDto.IdGameType = newGameType.IdgameType;

                return Ok(gameTypeDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateGameType")]
        public ActionResult<GameTypeDto> PutGameType(int id, [FromBody] GameTypeDto updatedGameTypeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var UpdatedGameType = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                if (UpdatedGameType == null)
                {
                    return NotFound();
                }

                UpdatedGameType.GameTypeName = updatedGameTypeDto.GameTypeName;
                UpdatedGameType.Description = updatedGameTypeDto.Description;

                _context.SaveChanges();

                return Ok(UpdatedGameType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteGameType")]
        public ActionResult<GameTypeDto> DeleteGameType(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var gameTypeToDelete = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                if (gameTypeToDelete == null)
                {
                    return NotFound();
                }

                var gamesWithGameType = _context.Games.Where(x => x.GameTypeId == id).ToList();

                foreach (var game in gamesWithGameType)
                {
                    _context.Games.Remove(game);
                }

                _context.GameTypes.Remove(gameTypeToDelete);

                _context.SaveChanges();

                return Ok(gameTypeToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
