namespace Ordering.Domain.Models.ValueObjects;

public record Payment
{
    public string CardNumber { get; init; } = default!;
    public string? CardName { get; init; } = default!;
    public string Expiration { get; init; } = default!;
    public string CVV { get; init; } = default!;
    public int PaymentMethod { get; init; } = default!;
    
    protected Payment() { }
    
    private Payment(string cardNumber,
        string cardName,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        CardNumber = cardNumber;
        CardName = cardName;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }
    
    public static Payment Of(string cardNumber,
        string cardName,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfNotEqual(cvv.Length, 3);
        
        return new Payment(cardNumber, cardName, expiration, cvv, paymentMethod);
    }
}