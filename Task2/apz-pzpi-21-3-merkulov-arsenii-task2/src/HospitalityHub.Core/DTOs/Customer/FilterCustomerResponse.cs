namespace HospitalityHub.Core.DTOs.Customer;

public record FilterCustomerResponse
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
}