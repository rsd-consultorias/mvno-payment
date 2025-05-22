using Microsoft.AspNetCore.Mvc;

namespace Controller;

[ApiController]
[Route("/api/auth/.well-known")]
public class WellKnownController : ControllerBase
{
    [HttpGet("openid-configuration")]
    public IActionResult GetOpenIdConfiguration()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var configuration = new
        {
            issuer = $"{baseUrl}", // URL do emissor
            authorization_endpoint = $"{baseUrl}/api/auth/authorize",
            token_endpoint = $"{baseUrl}/api/auth/token",
            userinfo_endpoint = $"{baseUrl}/api/auth/userinfo",
            permissions_endpoint = $"{baseUrl}/api/auth/permissions",
            jwks_uri = $"{baseUrl}/api/auth/.well-known/jwks.json",
            response_types_supported = new[] { "code", "token", "id_token" },
            grant_types_supported = new[] { "authorization_code", "client_credentials", "refresh_token" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256", "HS256" },
            scopes_supported = new[] { "openid", "profile", "email", "read:items", "write:items" },
            token_endpoint_auth_methods_supported = new[] { "client_secret_post" },
            claims_supported = new[] { "sub", "name", "email", "preferred_username", "picture" }
        };

        return Ok(configuration);
    }

    [HttpGet("jwks.json")]
    public IActionResult GetJwks()
    {
        var keys = new[]
        {
        new
        {
            kty = "RSA",
            use = "sig",
            kid = "12345", // ID da chave
            alg = "RS256",
            n = "base64url-modulus", // Modulus da chave pública
            e = "AQAB" // Expoente da chave pública
        }};

        return Ok(new { keys });
    }
}
