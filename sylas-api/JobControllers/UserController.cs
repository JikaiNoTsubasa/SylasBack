using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.UserModel;

namespace sylas_api.JobControllers;

[Authorize]
public class UserController(SyContext context, UserManager userManager) : SyController(context)
{
    protected UserManager _userManager = userManager;

    [HttpGet]
    [Route("api/users")]
    public IActionResult FetchUsers(){
        return Return(_userManager.FetchUsersFiltered(_pagination, _search, _orderby));
    }

    [HttpGet]
    [Route("api/user/{id}")]
    public IActionResult FetchUser([FromRoute] long id){
        var res = new ApiResult(){
            Content = _userManager.FetchUser(id).ToDTO(),
            HttpCode = StatusCodes.Status200OK
        };

        return Return(res);
    }

    [HttpGet]
    [Route("api/user/me")]
    public IActionResult FetchMyUser(){
        var res = new ApiResult(){
            Content = _userManager.FetchUser(_loggedUserId).ToDTO(),
            HttpCode = StatusCodes.Status200OK
        };

        return Return(res);
    }

    [HttpPost]
    [Route("api/user")]
    public IActionResult CreateUser([FromForm] RequestCreateUser model){
        User user = _userManager.CreateUser(model.Email, model.Name, _loggedUserId, model.Password, model.Avatar, model.Street, model.City, model.Zipcode, model.Country);
        var res = new ApiResult(){ Content = user.ToDTO(), HttpCode = StatusCodes.Status201Created };
        return Return(res);
    }

    [HttpPatch]
    [Route("api/user/{id}")]    
    public IActionResult UpdateUser([FromRoute] long id, [FromForm] RequestUpdateUser model){
        User user = _userManager.UpdateUser(id, _loggedUserId, model.Email, model.Name, model.Password, model.Avatar, model.Street, model.City, model.Zipcode, model.Country);
        var res = new ApiResult(){ Content = user.ToDTO(), HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }

    [HttpDelete]
    [Route("api/user/{id}")]
    public IActionResult DeleteUser([FromRoute] long id){
        _userManager.DeleteUser(id, _loggedUserId);
        var res = new ApiResult(){ HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }
}
