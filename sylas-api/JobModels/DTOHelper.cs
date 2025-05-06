using System;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobModels.GlobalParameterModel;
using sylas_api.JobModels.PreferenceModel;
using sylas_api.JobModels.ProjectModel;
using sylas_api.JobModels.TimeModel;
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
            XPManagement = model.XPManagement,
            LevelBackEnd = model.LevelBackEnd,
            LevelFrontEnd = model.LevelFrontEnd,
            LevelTests = model.LevelTests,
            LevelManagement = model.LevelManagement,
            XpPercentFrontEnd = Engine.GetCurrentLevelXpPercent(model.LevelFrontEnd, model.XPFrontEnd),
            XpPercentBackEnd = Engine.GetCurrentLevelXpPercent(model.LevelBackEnd, model.XPBackEnd),
            XpPercentTests = Engine.GetCurrentLevelXpPercent(model.LevelTests, model.XPTests),
            XpPercentManagement = Engine.GetCurrentLevelXpPercent(model.LevelManagement, model.XPManagement),
            Preferences = model.Preferences?.ToDTO(),
            LastConnection = model.LastConnection,
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

#region Preference
    public static ResponsePreference ToDTO(this Preferences model)
    {
        return new(){
            Id = model.Id,
            Name = model.Name,
            TimeHistory = model.TimeHistory,
            TimeChartMonth = model.TimeChartMonth,
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
            Customer = model.Customer?.ToDTO(),
            Issues = model.Issues?.Select(i => i.ToDTO()).ToList(),
            IsDeleted = model.IsDeleted,
            Status = model.Status,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }

    public static ResponseIssue ToDTO(this Issue model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            GitlabTicket = model.GitlabTicket,
            Complexity = model.Complexity,
            DevelopmentTime = model.DevelopmentTime,
            DueDate = model.DueDate,
            IsDeleted = model.IsDeleted,
            Labels = model.Labels?.Select(l => l.ToDTO()).ToList(),
            Milestone = model.Milestone?.ToDTO(),
            Priority = model.Priority,
            Status = model.Status,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }

    public static ResponseMilestone ToDTO(this Milestone model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name
        };
    }

    public static ResponseLabel ToDTO(this Label model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }
#endregion

#region Customer
    public static ResponseCustomer ToDTO(this Customer model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }
#endregion
#region Time
    public static ResponseTime ToDTO(this DayTime model)
    {
        return new()
        {
            Id = model.Id,
            User = model.User?.ToDTO()!,
            Date = model.Date,
            Minutes = model.Minutes,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
            DeletedDate = model.DeletedDate,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy
        };
    }
#endregion

#region GlobalParameter
    public static ResponseGlobalParameter ToDTO(this GlobalParameter model)
    {
        return new()
        {
            Id = model.Id,
            Name = model.Name,
            Value = model.Value,
            Type = model.Type,
            Description = model.Description
        };
    }
#endregion
}
