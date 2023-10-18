using System.Security.Claims;
using AutoMapper;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintyAPI.Models;
using PaintyAPI.Models.User;
using PaintyAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PaintyAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;

    public UserController(UserService userService, IMapper mapper, TokenService tokenService)
    {
        _userService = userService;
        _mapper = mapper;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(int?), 200)]
    [ProducesResponseType(typeof(string), 500)]
    [SwaggerOperation("Register a new user")]
    public async Task<ActionResult<int?>> RegisterUser(UserRegister user)
    {
        try
        {
            var userDomain = _mapper.Map<Domain.Models.User>(user);
            
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            userDomain.RefreshToken = refreshToken;
            userDomain.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(31);
            
            var userNew = await _userService.RegisterAsync(userDomain);
            
            var claims = new List<Claim>
            {
                new Claim("Id", userNew.Id.ToString() ?? ""),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);

            var jwt = new Jwt { AccessToken = accessToken, RefreshToken = refreshToken };
            
            return Ok(jwt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 500)]
    [SwaggerOperation("Authenticate a user")]
    public async Task<ActionResult<UserResponse>> AuthenticateUser(UserAuth user)
    {
        try
        {
            var userDomain = _mapper.Map<Domain.Models.User>(user);
            
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            userDomain.RefreshToken = refreshToken;
            userDomain.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(31);
            
            userDomain = await _userService.AuthenticateUserAsync(userDomain);

            var claims = new List<Claim>
            {
                new Claim("Id", userDomain!.Id.ToString() ?? ""),
            };
        
            var accessToken = _tokenService.GenerateAccessToken(claims);
            
            Jwt jwt = new Jwt { AccessToken = accessToken, RefreshToken = refreshToken };
            return Ok(jwt);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("add-friend"), Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [SwaggerOperation("Add a friend")]
    public async Task<IActionResult> AddFriend([FromBody] int recipientId)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            
            int userId = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);

            var sender = await _userService.AddFriendAsync(userId!, recipientId!);

            return Ok(_mapper.Map<UserResponse>(sender));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("get-all"), Authorize]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), 200)]
    [ProducesResponseType(typeof(string), 500)]
    [SwaggerOperation("Get all users")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
    {
        try
        {
            var users = await _userService.GetAllAsync();

            var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
        
            return Ok(userResponses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    
}