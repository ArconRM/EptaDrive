using EptaDrive.Entities;
using EptaDrive.Entities.Requests;
using EptaDrive.Utlis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = EptaDrive.Entities.Requests.LoginRequest;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EptaDrive.Controllers;

[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        SignInResult result = await _signInManager.PasswordSignInAsync(
            loginRequest.Login,
            loginRequest.Password,
            loginRequest.RememberMe,
            false);

        if (!result.Succeeded)
            return Unauthorized();

        return Ok();
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest registerRequest)
    {
        if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
            return BadRequest("Email already exists");

        if (await _userManager.FindByNameAsync(registerRequest.Login) != null)
            return BadRequest("Login already exists");

        var user = new User
        {
            UserName = registerRequest.Login,
            Email = registerRequest.Email
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        return Ok();
    }

    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}