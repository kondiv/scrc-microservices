namespace EquipmentService.Api.Common;

public class PaginatedListResponse<T>
{
    public IEnumerable<T> Items { get; }
    public string? CursorToken { get; }
    public bool HasMore { get; }
    
    public PaginatedListResponse(IEnumerable<T> items, string? cursorToken, bool hasMore)
    {
        Items = items;
        CursorToken = cursorToken;
        HasMore = hasMore;
    }
}