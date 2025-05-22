namespace Core.Model;

public record CreditCard : IPaymentMethod
{
    public required string CardToken { get; set; }
    public required string Brand { get; set; }
    public required string LastDigits { get; set; }
    public required string Expiry { get; set; }
    public required string NameOnCard { get; set; }
    public required Address BillingAddress { get; set; }
}