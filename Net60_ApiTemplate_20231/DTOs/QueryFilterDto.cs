namespace Net60_ApiTemplate_20231.DTOs
{
    public record QueryFilterDto(
        string? Column = null,
        string? Contain = null
    );

    public record QuerySortDto(
        string? SortColumn = null,
        string? Ordering = "asc"
    );
}