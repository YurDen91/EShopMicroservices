namespace Ordering.Domain.Models.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; set; }
    private OrderName(string value) => Value = value;
    
    public static OrderName Of(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

        return new OrderName(value);
    }
}