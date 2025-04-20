using System;
using sylas_api.Database.Models;
using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels;

public static class DTOHelper
{
#region User
    public static ResponseUser ToDTO(this User user)
    {
        return new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            XPBackEnd = user.XPBackEnd,
            XPFrontEnd = user.XPFrontEnd,
            XPTests = user.XPTests,
            LevelBackEnd = user.LevelBackEnd,
            LevelFrontEnd = user.LevelFrontEnd,
            LevelTests = user.LevelTests
        };
    }
#endregion
}
