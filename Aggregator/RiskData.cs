using EnsureThat;

namespace Aggregator;

public sealed record RiskData
{
    public RiskData(string firstName, string lastName, decimal value, string? make, DateTime? dob)
    {
        EnsureArg.IsNotNullOrEmpty(firstName, nameof(firstName));
        EnsureArg.IsNotNullOrEmpty(lastName, nameof(lastName));
        EnsureArg.IsNotDefault(value, nameof(value));

        FirstName = firstName;
        LastName = lastName;
        Value = value;
        Make = make;
        DOB = dob;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public decimal Value { get; }
    public string? Make { get; }
    public DateTime? DOB { get; }
}