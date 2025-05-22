namespace Core.Model;

public record ServiceInfoVO
{
    public bool Active { get; set; }
    public Guid CorrelationId { get; set; }
}