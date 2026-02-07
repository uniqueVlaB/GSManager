using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthentificationController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        throw new NotImplementedException();
    }
}
