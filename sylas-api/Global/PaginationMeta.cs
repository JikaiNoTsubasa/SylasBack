using System;

namespace sylas_api.Global;

public class PaginationMeta
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = -1;
    public int Limit { get; set; } = -1;
    public int PageCount {
            get{
                if(Limit <= 0 || Total <= 0 || PageSize <= 0)
                    return 1;
                return (int)Math.Ceiling(Total / (double)Limit);
            }
        }
    public int Total { get; set; } = -1;

    public Dictionary<string, string> GenerateDictionary()
    {
        Dictionary<string, string> dict = new()
        {
            { "X-Pagination-CurrentPage", Page.ToString()},
            { "X-Pagination-PageSize", PageSize.ToString() },
            { "X-Pagination-TotalPages", PageCount.ToString() },
            { "X-Pagination-TotalCount", Total.ToString() }
        };
        return dict;
    }
}
