namespace DTO;

public record ServiceDTO
{
    public Guid Id { get; set; }
    public string name { get; set; }
    public string Description { get; set; }
}