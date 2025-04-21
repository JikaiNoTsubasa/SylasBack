using System;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;
using sylas_api.JobModels;
using sylas_api.JobModels.UserModel;
using sylas_api.Services;

namespace sylas_api.JobManagers;

public class UserManager(SyContext context, HashService hashService) : SyManager(context)
{
    protected HashService _hashService = hashService;

    public List<User> FetchUsers(){
        return [.. _context.Users];
    }

    public ApiResult FetchUsersFiltered(QueryableEx.Pagination? pagination, QueryableEx.SearchQuery? search, QueryableEx.OrderQuery? order){
        List<ResponseUser> users = [.. _context.Users
            .Search(search, u => u.Name, u=> u.Email)
            .OrderBy(order, "name", u => u.Name)
            .Paged(pagination, out var meta)
            .Select(u => u.ToDTO())];
        return new ApiResult { Content = users, Meta = meta, HttpCode = StatusCodes.Status200OK };
    }

    public User FetchUser(long userId){
        if (userId <= 0) throw new SyBadRequest($"Trying to get user {userId}");
        return _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new SyEntitiyNotFoundException($"User {userId} not found");
    }

    public User CreateUser(
        string email,
        string name,
        long createdBy,
        string? password,
        string? avatar,
        string? street,
        string? city,
        string? zipcode,
        string? country
        ){
        User user = new(){
            Email = email,
            Password = _hashService.HashPassword(password ?? "test"),
            Name = name,
        };
        if (avatar != null) user.Avatar = avatar;
        if (street != null) user.Street = street;
        if (city != null) user.City = city;
        if (zipcode != null) user.Zipcode = zipcode;
        if (country != null) user.Country = country;
        user.MarkCreated(createdBy);

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User UpdateUser(long userId,long updatedBy, string? email, string? name, string? password, string? avatar, string? street, string? city, string? zipcode, string? country){
        User user = FetchUser(userId);
        if (email != null) user.Email = email;
        if (name != null) user.Name = name;
        if (avatar != null) user.Avatar = avatar;
        if (password != null) user.Password = _hashService.HashPassword(password);
        if (street != null) user.Street = street;
        if (city != null) user.City = city;
        if (zipcode != null) user.Zipcode = zipcode;
        if (country != null) user.Country = country;
        user.MarkUpdated(updatedBy);
        _context.SaveChanges();
        return user;
    }

    public void DeleteUser(long userId, long deletedBy){
        User user = FetchUser(userId);
        user.Email = $"Deleted-{user.Email}-{user.Id}";
        user.Name = $"{user.Name} [DELETED]";
        user.MarkDeleted(deletedBy);
        _context.SaveChanges();
    }
}
