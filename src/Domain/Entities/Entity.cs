
namespace Domain.Entities;

public interface IEntity
{
    int Id { get; }
}

public interface IEntity<TPrimaryKey>
    where TPrimaryKey : struct
{
    TPrimaryKey Id { get; }
}

public abstract class Entity : IEntity
{
    public virtual int Id { get; init; }
}

public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    where TPrimaryKey : struct
{
    public virtual TPrimaryKey Id { get; init; }
}
