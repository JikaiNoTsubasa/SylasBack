using System;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;

namespace sylas_api.JobManagers;

public class PreferenceManager(SyContext context) : SyManager(context)
{
    public Preferences FetchMyPreferences(long userId) {
        Preferences pref = _context.Preferences.FirstOrDefault(p => p.UserId == userId) ?? throw new SyEntitiyNotFoundException($"Preferences for user {userId} not found");
        return pref;
    }
}
