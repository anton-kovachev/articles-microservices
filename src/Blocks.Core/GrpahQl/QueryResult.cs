namespace Blocks.Core.GrpahQl;

public class QueryResult<T>
{
    public IEnumerable<T> Items { get; init; } = null!;
    public int Count { get; set; }
    public int PageNumber { get; set; }
}
