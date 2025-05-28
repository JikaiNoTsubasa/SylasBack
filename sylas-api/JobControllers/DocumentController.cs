using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.Database.Models;
using sylas_api.JobModels.DocumentModel;

namespace sylas_api.JobControllers;

[Authorize]
public class DocumentController(SyContext context, DocumentManager documentManager) : SyController(context)
{
    protected DocumentManager _documentManager = documentManager;

    [HttpPost]
    [Route("api/document")]
    public IActionResult CreateDocument([FromForm] RequestCreateDocument model)
    {
        Document doc = _documentManager.CreateDocument(model.Name, model.File, _loggedUserId, model.EntityId).Result;
        return Return(new ApiResult() { Content = doc.ToDTO(), HttpCode = StatusCodes.Status201Created });
    }

    [HttpGet]
    [Route("api/documents")]    
    public IActionResult GetDocument([FromQuery] long? entityId = null)
    {
        List<ResponseDocument> docs = [.. _documentManager.FetchDocuments(entityId).Select(d => d.ToDTO())];
        return Return(new ApiResult() { Content = docs, HttpCode = StatusCodes.Status200OK });
    }
}
