using System;

namespace sylas_api.Global;

public class ApiResult
{
    public int HttpCode { get; set; }
    public object? Content { get; set; }
    public PaginationMeta? Meta { get; set; }
}
