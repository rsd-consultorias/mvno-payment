using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utils;

public class TokenValidator
{
    private const string Issuer = "https://identity-server"; // Emissor esperado
    private const string Audience = "http://localhost/api"; // Audiência esperada
    private const string SecretKey = "a-string-secret-at-least-256-bits-long-abcdef1234567890ABCDEFGHIJKLMN7890"; // Chave secreta usada na assinatura do token

    public static bool ValidateAccessToken(string token, out ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal = null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer, // Emissor esperado
                ValidAudience = Audience, // Audiência esperada
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), // Chave secreta
                ValidateIssuer = true, // Valida emissor
                ValidateAudience = true, // Valida audiência
                ValidateLifetime = true, // Valida expiração do token
                ValidateIssuerSigningKey = true // Valida assinatura do token
            };

            // Valida o token e retorna os claims
            claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Se chegou aqui, o token é válido
            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            // Token expirado
            Console.WriteLine("Token expired");
            return false;
        }
        catch (SecurityTokenException)
        {
            // Token inválido
            Console.WriteLine("Token validation failed");
            return false;
        }
        catch (Exception ex)
        {
            // Outros erros
            Console.WriteLine($"An error occurred during validation: {ex.Message}");
            return false;
        }
    }
}
