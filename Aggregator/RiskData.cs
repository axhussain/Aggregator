using EnsureThat;

namespace Aggregator;

public sealed record RiskData
{
    public RiskData(string firstName, string lastName, string value, string? make, string? dob = null)
    {
        EnsureArg.IsNotNullOrEmpty(firstName, nameof(firstName));
        EnsureArg.IsNotNullOrEmpty(lastName, nameof(lastName));
        EnsureArg.IsNotNullOrEmpty(value, nameof(value));

        FirstName = firstName;
        LastName = lastName;
        Make = make ?? string.Empty;

        if (decimal.TryParse(value, out decimal parsedValue))
        {
            EnsureArg.IsNot(parsedValue, 0, nameof(parsedValue));
            Value = parsedValue;
        }

        if (DateTime.TryParse(dob, out DateTime parsedDob))
        {
            DOB = parsedDob;
        }
    }

    public string FirstName { get; }
    public string LastName { get; }
    public decimal Value { get; }
    public string Make { get; }
    public DateTime? DOB { get; }
}