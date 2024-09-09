using GameCritic.DTOModels;
using GameCriticBL.Models;
using GameCritic.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGamersController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        private readonly IConfiguration _configuration;

        public UserGamersController(RwaprojectDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public ActionResult<UserGamerRegisterDto> Register(UserGamerRegisterDto registerDto)
        {
            try
            {
                var trimmedUsername = registerDto.Username.Trim();
                if (_context.UserGamers.Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                var userRole = _context.UserRoles.FirstOrDefault(x => x.RoleName == "User");

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(registerDto.Password, b64salt);

                var user = new UserGamer
                {
                    IduserGamer = registerDto.IduserGamer,
                    Username = registerDto.Username,
                    PwdHash = b64hash,
                    PwdSalt = b64salt,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    City = registerDto.City,
                    PostalCode = registerDto.PostalCode,
                    HomeAddress = registerDto.HomeAddress,
                    UserRoleId = userRole.IduserRole
                };

                _context.Add(user);
                _context.SaveChanges();

                registerDto.IduserGamer = user.IduserGamer;

                return Ok(registerDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login")]
        public ActionResult Login(UserGamerSignInDto signInDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingUser = _context.UserGamers.Include(x => x.UserRole).FirstOrDefault(x => x.Username == signInDto.Username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                var b64hash = PasswordHashProvider.GetHash(signInDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                var secureKey = _configuration["JWT:SecureKey"];

                var serializedToken =
                    JwtTokenProvider.CreateToken(
                        secureKey,
                        120,
                        signInDto.Username,
                        existingUser.UserRole.RoleName);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ChangePassword")]
        public ActionResult<UserGamerChangePasswordDto> ChangePassword(UserGamerChangePasswordDto changePasswordDto)
        {
            try
            {
                var trimmedUsername = changePasswordDto.Username.Trim();
                var existingUser = _context.UserGamers.FirstOrDefault(x => x.Username.Equals(trimmedUsername));
                if (existingUser == null)
                    return BadRequest($"Username {trimmedUsername} does not exist");

                existingUser.PwdSalt = PasswordHashProvider.GetSalt();
                existingUser.PwdHash = PasswordHashProvider.GetHash(changePasswordDto.Password, existingUser.PwdSalt);

                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("PromoteUser")]
        public ActionResult<UserGamerRegisterDto> PromoteUser(UserGamerPromoteDto promoteDto)
        {
            try
            {
                var trimmedUsername = promoteDto.Username.Trim();
                var existingUser = _context.UserGamers.FirstOrDefault(x => x.Username.Equals(trimmedUsername));
                if (existingUser == null)
                    return BadRequest($"Username {trimmedUsername} does not exist");

                var adminRole = _context.UserRoles.FirstOrDefault(x => x.RoleName == "Admin");

                existingUser.UserRoleId = adminRole.IduserRole;

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAllUserGamers")]
        public ActionResult<IEnumerable<UserGamerDto>> GetAllUserGamers()
        {
            try
            {
                var result = _context.UserGamers;
                var mappedResult = result.Select(x =>
                    new UserGamerDto
                    {
                        IduserGamer = x.IduserGamer,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        City = x.City,
                        PostalCode = x.PostalCode,
                        HomeAddress = x.HomeAddress,
                        Username = x.Username
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserGamerDto> GetUserGamerById(int id)
        {
            try
            {
                var result =
                    _context.UserGamers
                        .FirstOrDefault(x => x.IduserGamer == id);

                var mappedResult = new UserGamerDto
                {
                    IduserGamer = result.IduserGamer,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Email = result.Email,
                    City = result.City,
                    PostalCode = result.PostalCode,
                    HomeAddress = result.HomeAddress,
                    Username = result.Username,
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public ActionResult<UserGamerDto> PutUser(int id, [FromBody] UserGamerDto updatedUserGamerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedUserGamer = _context.UserGamers.FirstOrDefault(x => x.IduserGamer == id);
                if (updatedUserGamer == null)
                {
                    return NotFound();
                }

                updatedUserGamer.FirstName = updatedUserGamerDto.FirstName;
                updatedUserGamer.LastName = updatedUserGamerDto.LastName;
                updatedUserGamer.Email = updatedUserGamerDto.Email;
                updatedUserGamer.City = updatedUserGamerDto.City;
                updatedUserGamer.PostalCode = updatedUserGamerDto.PostalCode;
                updatedUserGamer.HomeAddress = updatedUserGamerDto.HomeAddress;
                updatedUserGamer.Username = updatedUserGamerDto.Username;

                _context.SaveChanges();

                return Ok(updatedUserGamer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteUser")]
        public ActionResult<UserGamerDto> DeleteUser(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userGamerToDelete = _context.UserGamers.FirstOrDefault(x => x.IduserGamer == id);
                if (userGamerToDelete == null)
                {
                    return NotFound();
                }

                _context.UserGamers.Remove(userGamerToDelete);

                _context.SaveChanges();

                return Ok(userGamerToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
