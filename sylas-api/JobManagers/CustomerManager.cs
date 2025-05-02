using System;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class CustomerManager(SyContext context) : SyManager(context)
{
    public List<Customer> FetchCustomers()
    {
        return [.. _context.Customers];
    }
}
