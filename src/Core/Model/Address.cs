namespace Core.Model;

public record Address
{
    public required string PostalCode { get; set; }
    public required string Street { get; set; }
    public required string Locality { get; set; }
    public required string Region { get; set; }
    public required string Country { get; set; }
    public bool BillingAddres { get; set; }
}