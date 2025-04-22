using System;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobModels.ProjectModel;
using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels;

public static class DTOHelper
{
#region User
    public static ResponseUser ToDTO(this User model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            Email = model.Email,
            XPBackEnd = model.XPBackEnd,
            XPFrontEnd = model.XPFrontEnd,
            XPTests = model.XPTests,
            LevelBackEnd = model.LevelBackEnd,
            LevelFrontEnd = model.LevelFrontEnd,
            LevelTests = model.LevelTests,
            XpPercentFrontEnd = Engine.GetCurrentLevelXpPercent(model.LevelFrontEnd, model.XPFrontEnd),
            XpPercentBackEnd = Engine.GetCurrentLevelXpPercent(model.LevelBackEnd, model.XPBackEnd),
            XpPercentTests = Engine.GetCurrentLevelXpPercent(model.LevelTests, model.XPTests),
            Avatar = model.Avatar,
            Street = model.Street,
            City = model.City,
            Zipcode = model.Zipcode,
            Country = model.Country,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }
#endregion

#region Project
    public static ResponseProject ToDTO(this Project model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            Owner = model.Owner?.ToDTO(),
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }
#endregion
}
