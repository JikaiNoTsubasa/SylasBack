using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return StatusCode(StatusCodes.Status200OK, new ResponseLogin { Token = token });
        
    }
}
