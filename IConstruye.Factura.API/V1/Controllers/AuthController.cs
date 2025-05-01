using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IConstruye.Factura.V1.Controllers;

public class AuthController(IConfiguration configuration) : ApiController
{
    private IConfiguration Configuration { get; } = configuration;
    
    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult GetToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Configuration.GetSection("TokenSettings")["ClaveSecreta"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = Configuration.GetSection("TokenSettings")["Issuer"],
            Audience = Configuration.GetSection("TokenSettings")["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { token = tokenString });
    }
}