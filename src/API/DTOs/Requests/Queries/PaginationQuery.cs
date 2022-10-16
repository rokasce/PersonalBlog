namespace API.DTOs.Requests.Queries;

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
		PageSize = pageSize > 100 ? 100 : pageNumber;
	}

	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}
