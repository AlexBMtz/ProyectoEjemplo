using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.Models.DTO.OutputDTO;

namespace TestMAUIAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserManager<User> userManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<LoggedInUserOutputDTO>> GetLoggedInUser()
        {
            User? loggedInUser = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(loggedInUser);
            var currentRole = roles.FirstOrDefault(r => User.IsInRole(r));

            return Ok(new LoggedInUserOutputDTO
            {
                FirstName = loggedInUser.FirstName,
                LastName = loggedInUser.LastName,
                Role = currentRole
            });
        }
    }
}
