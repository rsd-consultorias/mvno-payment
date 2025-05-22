using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adiciona o Swagger

// Configuração do JWT
var key = Encoding.ASCII.GetBytes("a-string-secret-at-least-256-bits-long-abcdef1234567890ABCDEFGHIJKLMN7890");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Use true em produção
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = "http://localhost/api",
        ValidIssuer = "https://identity-server",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true, // Configure para true em produção
        ValidateAudience = true, // Configure para true em produção
        ClockSkew = TimeSpan.Zero
    };
    // Configuração dos eventos de autenticação
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var fingerprint = Convert.ToBase64String(Encoding.UTF8.GetBytes(userAgent));

            var tokenFingerprint = context.Principal?.Claims.FirstOrDefault(c => c.Type == "fingerprint")?.Value;

            if (tokenFingerprint != fingerprint)
            {
                context.Fail("Fingerprint inválida! O token não pertence a este dispositivo.");
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers(); // Adiciona os controladores

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger e o Swagger UI no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseReDoc(options =>
        {
            options.DocumentTitle = "Catalog API";
            options.RoutePrefix = "docs"; // Define o endpoint para acessar a documentação
            options.SpecUrl = "/swagger/v1/swagger.json"; // URL do arquivo Swagger
            options.HideDownloadButton(); // Opcional: Esconde o botão de download
            options.HideHostname(); // Opcional: Esconde o hostname
        });
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Middleware de autenticação
app.UseAuthorization(); // Middleware de autorização
app.MapControllers(); // Mapeia os controladores

app.Run();
