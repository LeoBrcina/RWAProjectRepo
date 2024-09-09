using GameCriticBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogItemsController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        private readonly IConfiguration _configuration;
        public LogItemsController(RwaprojectDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("GetLogCount")]
        [Authorize]
        public ActionResult<IEnumerable<LogItem>> GetLogCount()
        {
            try
            {
                return Ok(_context.LogItems.Count());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllLogs")]
        [Authorize]
        public ActionResult<IEnumerable<LogItem>> GetAllLogs()
        {
            try
            {
                return Ok(_context.LogItems);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<LogItem> GetLogById(int id)
        {
            try
            {
                var log =
                    _context.LogItems
                    .FirstOrDefault(x => x.Idlog == id);
                if (log == null)
                {
                    return NotFound($"Could not find log with id {id}");
                }

                return Ok(log);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
