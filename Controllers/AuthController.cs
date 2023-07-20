using DotNetCoreJwtExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreJwtExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpPost("token")]
    public IResult Token(GetTokenModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Email.Equals("dummy@dummy.com") && model.Password.Equals("1234567890"))
            {

                var tokenStr = CreateToken(model);
                return Results.Ok(new { token = tokenStr });
            }
            else
            {
                return Results.Unauthorized();
            }
        }
        else
        {
            return Results.BadRequest(ModelState);
        }


    }

    [NonAction]
    public string CreateToken(GetTokenModel model)
    {

        var securtyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
        var credentials = new SigningCredentials(securtyKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
        new Claim(ClaimTypes.Email, model.Email),
    };

        var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
            _configuration["JwtConfig:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
