using System;
using log4net;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Services;

namespace sylas_api.Global;

public class SyProjectInit
{
    private static readonly ILog log = LogManager.GetLogger(typeof(SyProjectInit));

    public static void CreateAdminIfNotExist(SyContext context, HashService hashService){
        // Check if admin exists
        if (!context.Users.Any()){
            log.Info("Init default admin");   
            var password = hashService.HashPassword("test");
            var user = new User { Email = "stephane.biehler.priv@gmail.com", Password = password, Name = "Jikai" };
            user.MarkCreated(0);
            context.Users.Add(user);
            context.SaveChanges();
            log.Debug($"Admin {user.Name} created");
        }
    }

    public static void InitGlobalParameters(SyContext context){
        // Create param Bearer expirancy
        CreateParameterIfNotExist(
            context,
            SyApplicationConstants.PARAM_BEARER_EXPIRATION_HOURS,
            "1",
            "int",
            "Bearer token expiration in hours"
        );

        // Create param Create Project XP
        CreateParameterIfNotExist(
            context,
            SyApplicationConstants.PARAM_PROJECT_CREATE_XP,
            "40",
            "int",
            "XP gained when creating a project"
        );
    }

    public static void CreateParameterIfNotExist(SyContext context, string parameterName, string parameterValue, string parameterType, string? parameterDesc = null){
        log.Debug($"Checking if parameter {parameterName} exists...");
        // Check if param already exists
        if (!context.GlobalParameters.Any(p => p.Name.Equals(parameterName))){
            GlobalParameter param = new(){
                Name = parameterName,
                Value = parameterValue,
                Type = parameterType,
                Description = parameterDesc
            };
            context.GlobalParameters.Add(param);
            context.SaveChanges();
            log.Debug($"Parameter {parameterName} created");
        }else{
            log.Debug($"Parameter {parameterName} has value {parameterValue}");
        }
    }

    public static void InitUserPreferences(SyContext context){
        // For each users, create default preferences
        foreach (User user in context.Users.Include(u => u.Preferences).ToList()){
            if (user.Preferences == null){
                user.Preferences = new Preferences
                {
                    Name = $"Default preferences for {user.Name}"
                };
                context.SaveChanges();
            }
        }
    }

    public static void InitDefaultCustomers(SyContext context){
        // Create default customer
        if (!context.Customers.Any(c => c.Name.Equals("Personal"))){
            Customer customer = new()
            {
                Name = "Personal"
            };
            context.Customers.Add(customer);
            context.SaveChanges();
            log.Debug($"Customer {customer.Name} created");
        }
    }

    public static void InitGrants(SyContext context, HashSet<string> policies){
        // Create default grants
        foreach (string policy in policies){
            if (!context.Grants.Any(g => g.Key.Equals(policy))){
                Grant grant = new()
                {
                    Key = policy
                };
                context.Grants.Add(grant);
                context.SaveChanges();
                log.Debug($"Grant {policy} created");
            }
        }
    }

    public static void InitAdminRole(SyContext context){
        // Create admin role
        if (!context.Roles.Any(r => r.UniqueKey.Equals("Admin"))){
            Role role = new()
            {
                Name = "Admin",
                UniqueKey = "Admin"
            };
            context.Roles.Add(role);
            context.SaveChanges();
            log.Debug($"Role {role.Name} created");

        }
        // Add all grants to Admin role
        Role admin = context.Roles.Include(r => r.Grants).First(r => r.UniqueKey.Equals("Admin"));
        admin.Grants = [.. context.Grants];
        context.SaveChanges();
        log.Debug($"Grants added to role {admin.Name}");
    }
}
