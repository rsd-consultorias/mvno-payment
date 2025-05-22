namespace DTO;

public record PlanDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public List<ServiceDTO> services { get; set; }
}