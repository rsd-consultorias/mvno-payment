namespace Core.Model;

public class CustomerInfo
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public required string FiscalIdentificationNumber { get; set; }
    public required string EMail { get; set; }
    public required string Phone { get; set; }
    public required ICollection<IPaymentMethod> PaymentMehtods { get; set; }
    public required ICollection<Address> Addresses { get; set; }
}