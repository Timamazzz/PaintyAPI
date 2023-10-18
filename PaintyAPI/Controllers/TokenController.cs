using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintyAPI.Models;
using PaintyAPI.Services;

namespace PaintyAPI.Controllers;


[ApiController]
[Route("tokens")]
public class TokenController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public TokenController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var refreshToken = HttpContext.Request.Headers["Refresh"].ToString();
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        
        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        int userId = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);

        var user = await _userService.GetByIdAsync(userId);

        if (user == null || user.RefreshToken != refreshToken)
            return BadRequest("Invalid client request");
        
        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return BadRequest("Token expired");
        
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime =  DateTime.UtcNow.AddDays(31);
        await _userService.UpdateAsync(user);

        Jwt newJwt = new Jwt
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshToken
        };

        return Ok(newJwt);
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var refreshToken = HttpContext.Request.Headers["Refresh"].ToString();
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        int userId = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);

        var user = await _userService.GetByIdAsync(userId);
        
        if (user == null || user.RefreshToken != refreshToken)
            return BadRequest("Invalid client request");

        user.RefreshToken = null;
        await _userService.UpdateAsync(user);

        return Ok();
    }
}