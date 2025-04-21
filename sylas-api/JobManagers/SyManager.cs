using System;
using sylas_api.Database;

namespace sylas_api.JobManagers;

public class SyManager(SyContext context)
{
    protected SyContext _context = context;
}
