using System.Text.Json;

namespace API.Extensions;

public static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User == null)
        {
            return string.Empty;
        }

        return httpContext.User.Claims.Single(x => x.Type == "id").Value;
    }

    public static void AddPaginationHeaders(this HttpResponse response,
        int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new 
        {
            currentPage,
            itemsPerPage,
            totalItems,
            totalPages
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}