using System;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using sylas_api.JobControllers;

namespace sylas_api.Global;

public class PolicyScanner
{
    public static HashSet<string> GetAllPoliciesFromControllers(Assembly asm)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Parcours tous les types du projet Web (contrôleurs)
        foreach (var t in asm.GetTypes())
        {
            if (!t.IsSubclassOf(typeof(SyController)))
                continue;
            // Actions ou Controller marqué par [Authorize]
            var classAuthorize = t.GetCustomAttributes<AuthorizeAttribute>(true);
            foreach (var attr in classAuthorize)
            {
                if (!string.IsNullOrWhiteSpace(attr.Policy))
                    result.Add(attr.Policy);
            }

            foreach (var method in t.GetMethods())
            {
                // Actions marquées par [Authorize]
                var methodAuthorize = method.GetCustomAttributes<AuthorizeAttribute>(true);
                foreach (var attr in methodAuthorize)
                {
                    // Console.WriteLine($"Scanning {t.Name}.{method.Name}: Policy -> {attr.Policy}");
                    if (!string.IsNullOrWhiteSpace(attr.Policy))
                        result.Add(attr.Policy);
                }
            }
        }
        return result;
    }
}
