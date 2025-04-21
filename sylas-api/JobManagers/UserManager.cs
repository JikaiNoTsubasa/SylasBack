using System;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;

namespace sylas_api.JobManagers;

public class UserManager(SyContext context) : SyManager(context)
{
    public User GetUser(long userId){
        if (userId <= 0) throw new SyBadRequest($"Trying to get user {userId}");
        return Context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new SyEntitiyNotFoundException($"User {userId} not found");
    }
}
