namespace Application.Core.Queries;

public class PaginationQuery
{
    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 20;
    }

    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize > 100 ? 100 : pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
