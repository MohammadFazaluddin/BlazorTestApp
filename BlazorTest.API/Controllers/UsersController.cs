using BlazorTest.Data.Interfaces;
using BlazorTest.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsers _usersRepository;
        ILogger<UsersController> _logger;
        IHttpContextAccessor _contextAccessor;
        SignInManager<IdentityUser> _signInManager;
        public UsersController(IUsers users, ILogger<UsersController> logger, IHttpContextAccessor contextAccessor,
            SignInManager<IdentityUser> sign)
        {
            _usersRepository = users;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _signInManager = sign;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginModel loginDetails)
        {
            try
            {
                var user = await _usersRepository.GetUserByEmail(loginDetails.Email);

                if(user == null)
                {
                    return NotFound($"Could not find user Email: {loginDetails.Email}");
                }

                var userValidated = await _usersRepository.ValidateUser(loginDetails);

                if (!userValidated)
                {
                    return Unauthorized("Invalid Password");
                }

                var role = await _usersRepository.GetUserRolesByEmail(loginDetails.Email, user);

                var userDetails = new UserModel()
                {
                    Email = user.Email!,
                    Username = user.UserName!,
                    Name = user.UserName!,
                    Password = "",
                    Role = role[0]
                };
               
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while retrieving data from the database");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var result = await _usersRepository.GetAllUsers(pageNumber, pageSize);

                if(result == null || !result.Any())
                {
                    return NotFound("Could not find any users");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while retrieving data from the database");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel user)
        {
            try
            {
                var result = await _usersRepository.AddUser(user);

                if(!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable,
                        $"Could not add user to the database, Error: {result.Errors.First().Description}");
                }

                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email },
                       result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while inserting to the database");
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var result = await _usersRepository.GetUserByEmail(email);

                if(result == null)
                {
                    return NotFound($"User was not found Email: {email}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while retrieving data from the database");
            }
        }

    }
}
