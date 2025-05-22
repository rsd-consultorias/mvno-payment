using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using DTO;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Utils;

namespace Controller;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("token")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult GenerateToken()
    {
        var clientId = Request.Form["client_id"];
        var clientSecret = Request.Form["client_secret"];
        var grantType = Request.Form["grant_type"];

        // Validação de entrada
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(grantType))
        {
            return BadRequest(new { error = "client_id, client_secret, and grant_type are required" });
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        if (grantType == "authorization_code") // Fluxo para gerar id_token
        {
            var idTokenClaims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Iss, "https://identity-server"),
            new Claim(JwtRegisteredClaimNames.Aud, clientId),
            new Claim(JwtRegisteredClaimNames.Sub, "user123"),
            new Claim(JwtRegisteredClaimNames.Name, "John Doe"),
            new Claim(JwtRegisteredClaimNames.Email, "johndoe@example.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var idToken = new JwtSecurityToken(
                issuer: "https://identity-server",
                audience: clientId,
                claims: idTokenClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return Ok(new
            {
                id_token = new JwtSecurityTokenHandler().WriteToken(idToken),
                token_type = "bearer",
                expires_in = 900 // Expiração do id_token em segundos
            });
        }
        else if (grantType == "client_credentials") // Fluxo para gerar access_token
        {
            var accessTokenClaims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Aud, clientId),
            new Claim(JwtRegisteredClaimNames.Sub, "app_client"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("scope", "read:items write:items")
        };

            var accessToken = new JwtSecurityToken(
                issuer: "https://identity-server",
                audience: "http://localhost/api",
                claims: accessTokenClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                token_type = "bearer",
                expires_in = 1800
            });
        }
        else
        {
            return BadRequest(new { error = "Invalid grant_type. Use 'authorization_code' or 'client_credentials'." });
        }
    }

    [HttpGet("authorize")]
    public IActionResult Authorize([FromQuery] string client_id, [FromQuery] string redirect_uri, [FromQuery] string state, [FromQuery] string scope)
    {
        // Validação dos parâmetros obrigatórios
        if (string.IsNullOrEmpty(client_id) || string.IsNullOrEmpty(redirect_uri))
        {
            return BadRequest(new { error = "client_id and redirect_uri are required" });
        }

        // Simula uma interface de login
        var authorizationCode = Guid.NewGuid().ToString(); // Gera um código único temporário
        var uri = $"{redirect_uri}?code={authorizationCode}&state={state}";

        return Redirect(uri); // Redireciona o usuário com o código gerado
    }

    // Endpoint de validação de token (SSO: usado por várias aplicações)
    [HttpPost("validate")]
    public IActionResult ValidateToken([FromHeader] string Authorization)
    {
        if (string.IsNullOrEmpty(Authorization) || !Authorization.StartsWith("Bearer "))
        {
            return Unauthorized(new { error = "Invalid or missing token" });
        }

        var token = Authorization.Substring("Bearer ".Length);
        if (!TokenValidator.ValidateAccessToken(token, out ClaimsPrincipal claims))
        {
            return Unauthorized(new { error = "Token validation failed" });
        }

        return Ok(new { message = "Token is valid", claims = claims.Claims.Select(c => new { c.Type, c.Value }) });
    }

    [HttpGet("permissions")]
    public IActionResult GetPermissions([FromQuery] string applicationId, [FromHeader] string Authorization)
    {
        // Validação do token
        if (string.IsNullOrEmpty(Authorization) || !Authorization.StartsWith("Bearer "))
        {
            return Unauthorized(new { error = "Invalid or missing token" });
        }

        var token = Authorization.Substring("Bearer ".Length);
        if (!TokenValidator.ValidateAccessToken(token, out ClaimsPrincipal claimsPrincipal))
        {
            return Unauthorized(new { error = "Token validation failed" });
        }

        // Simulando a recuperação de permissões do banco de dados ou outro serviço
        var permissions = new Dictionary<string, object>
        {
            ["appA"] = new[] { "read", "write" },
            ["appB"] = new[] { "manage_users", "view_reports" }
        };

        // Retornar apenas as permissões específicas para a aplicação solicitada
        if (!string.IsNullOrEmpty(applicationId) && permissions.ContainsKey(applicationId))
        {
            return Ok(new { application = applicationId, permissions = permissions[applicationId] });
        }

        // Retornar todas as permissões se nenhum applicationId for fornecido
        return Ok(new { permissions });
    }


    [HttpGet("userinfo")]
    public IActionResult GetUserInfo([FromHeader] string Authorization)
    {
        if (string.IsNullOrEmpty(Authorization) || !Authorization.StartsWith("Bearer "))
        {
            return Unauthorized(new { error = "Invalid or missing token" });
        }

        var token = Authorization.Substring("Bearer ".Length);

        // Validação do access_token
        if (!TokenValidator.ValidateAccessToken(token, out ClaimsPrincipal claimsPrincipal))
        {
            return Unauthorized(new { error = "Token validation failed" });
        }

        // Recupera informações do usuário a partir dos claims
        var userInfo = new
        {
            sub = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            name = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Name)?.Value,
            email = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
            preferred_username = claimsPrincipal.FindFirst("preferred_username")?.Value,
            picture = claimsPrincipal.FindFirst("picture")?.Value
        };

        return Ok(userInfo);
    }
}