namespace DTO;

public record TokenRequest
{
    public string GrantType { get; set; }
    public string Scope { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}