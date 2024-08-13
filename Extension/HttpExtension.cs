using DatingApp.Helpers;
using System.Text.Json;

namespace DatingApp.Extension
{
    public static class HttpExtension
    {
        public static void AddPaginationHeader<T>(this HttpResponse httpResponse,PageList<T> values)
        {
            PaginationHeader paginationHeader=new PaginationHeader(values.CurrentPage,values.PageSizes,values.Totalcount,values.TotalPage);
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            httpResponse.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            httpResponse.Headers.Append("Access-Control-Expose-Headers", "paginationHeader");
        } 
    }
}
