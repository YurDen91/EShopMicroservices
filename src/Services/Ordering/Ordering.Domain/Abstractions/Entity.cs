namespace Ordering.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; } = default!;

    public DateTime? CreatedAt { get; set; } = null;

    public string? CreatedBy { get; set; } = null;

    public DateTime? LastModified { get; set; } = null;

    public string? LastModifiedBy { get; set; } = null;
}