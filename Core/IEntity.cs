namespace todo_cqrs.Core
{
    /// <summary>
    /// Identity Map Pattern
    /// </summary>
    public interface IEntity
    {
        int Id { get; }
    }
}