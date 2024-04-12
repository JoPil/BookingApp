namespace Bookify.Domain.Apartments;

public sealed record Description(string Value)
{
    public static explicit operator string(Description description) =>description.Value;
};
