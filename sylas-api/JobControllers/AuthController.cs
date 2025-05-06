using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobModels.AuthModel;
using sylas_api.Services;

namespace sylas_api.JobControllers;

[AllowAnonymous]
public class AuthController(SyContext context, AuthService authService, HashService hashService) : SyController(context)
{
    private readonly AuthService _authService = authService;
    private readonly HashService _hashService = hashService;

    [HttpPost]
    [Route("api/auth/login")]
    public IActionResult Login([FromForm] RequestLogin requestLogin)
    {
        if (requestLogin.Login == null || requestLogin.Password == null)
        {
            return BadRequest();
        }


        var user = _context.Users.FirstOrDefault(u => u.Email == requestLogin.Login);
        if (user == null)
        {
            return BadRequest();
        }

        if (!_hashService.VerifyPassword(requestLogin.Password, user.Password))
        {
            return BadRequest();
        }

        string expirationHours = _context.GlobalParameters.FirstOrDefault(p => p.Name.Equals(SyApplicationConstants.PARAM_BEARER_EXPIRATION_HOURS))?.Value ?? "1";

        var token = _authService.GenerateToken(user, int.Parse(expirationHours));
        user.LastConnection = DateTime.UtcNow;
        _context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK, new ResponseLogin { Token = token });
        
    }

    [HttpPost]
    [Route("api/auth/google-login")]
    public async Task<IActionResult> GoogleLoginAsync([FromBody] RequestGoogleLogin model)
    {
        if (model.AccessToken == null)
        {
            return BadRequest();
        }

        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={model.AccessToken}");

        if (!response.IsSuccessStatusCode)
            return Unauthorized();

        var json = await response.Content.ReadAsStringAsync();
        var userInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(json);

        if (userInfo == null)
            return Unauthorized();

        // Check if user exists in database
        var user = _context.Users.FirstOrDefault(u => u.Email.Equals(userInfo.Email));
        if (user == null)
        {
            return Unauthorized();
        }

        string expirationHours = _context.GlobalParameters.FirstOrDefault(p => p.Name.Equals(SyApplicationConstants.PARAM_BEARER_EXPIRATION_HOURS))?.Value ?? "1";

        var token = _authService.GenerateToken(user, int.Parse(expirationHours));
        return StatusCode(StatusCodes.Status200OK, new ResponseLogin { Token = token });
    }
}
