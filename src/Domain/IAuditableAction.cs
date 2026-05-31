namespace Blocks.Domain;

public interface IAuditableAction
{
    DateTime CreatedOn { get; }
    int CreatedById { get; set; }
    string Action { get; }
}

public interface IAuditableAction<TActionType> : IAuditableAction
{
    TActionType ActionType { get; }
    string IAuditableAction.Action => ActionType!.ToString();

}
