namespace DTO;

public record ErrorResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
}