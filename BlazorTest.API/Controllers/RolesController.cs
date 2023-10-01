using BlazorTest.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace BlazorTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoles _rolesRepository;
        private ILogger<RolesController> _logger;
        public RolesController(IRoles roles, ILogger<RolesController> logger)
        {
            _rolesRepository = roles;
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                var result = await _rolesRepository.CreateRole(roleName);

                if(result == null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable,
                        $"Could not create role {roleName}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while inserting to the database");
            }
        }


        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRoleByName(string roleName)
        {
            try
            {
                var result = await _rolesRepository.DeleteRole(roleName);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable,
                        $"Could not delete role, Error: {result.Errors.First().Description}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured while removing from the database");
            }
        }

    }
}
