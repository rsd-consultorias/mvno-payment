namespace Core.Model;

public record PersonalDataVO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? EMail { get; set; }
    public DateOnly BirthDate { get; set; }
}