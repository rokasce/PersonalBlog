namespace API.DTOs.Responses
{
    public class PaginatedResponse<T>  
    {
        public PaginatedResponse()
        {

        }

        public PaginatedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string NextPage { get; set; } = default!;
        public string PreviousPage { get; set; } = default!;
    }
}
