using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;

namespace sylas_api.JobManagers;

public class DocumentManager(SyContext context, GlobalParameterManager globalParameterManager, FileManager fileManager) : SyManager(context)
{
    protected GlobalParameterManager _globalParameterManager = globalParameterManager;
    protected FileManager _fileManager = fileManager;

    public async Task<Document> CreateDocument(string name, IFormFile file, long userId, long? entityId = null)
    {
        // Get document storage path
        string documentStoragePath = _globalParameterManager.GetParameterValue(SyApplicationConstants.PARAM_DOCUMENT_STORAGE_PATH, "documents");
        string guid = Guid.NewGuid().ToString();

        string fullpath = Path.Combine(documentStoragePath, guid);

        // Physical save
        Directory.CreateDirectory(documentStoragePath);
        using (var stream = new FileStream(fullpath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        Document document = new()
        {
            Name = name,
            Versions = []
        };

        if (entityId != null) document.EntityId = entityId.Value;

        DocumentVersion documentVersion = new()
        {
            Document = document,
            Version = "1",
            IsCurrent = true,
            Path = fullpath
        };

        document.Versions.Add(documentVersion);

        document.MarkCreated(userId);
        _context.Documents.Add(document);
        _context.SaveChanges();

        // Index file
        IndexFile(name, fullpath);
        return document;
    }

    public List<Document> FetchDocuments(long? entityId = null)
    {
        List<Document> documents = [.. _context.Documents.Include(d => d.Versions).Where(entityId is not null, d => d.EntityId == entityId)];
        return documents;
    }

    public void IndexFile(string name, string filePath)
    {
        string luceneIndexPath = _globalParameterManager.GetParameterValue(SyApplicationConstants.PARAM_LUCENE_INDEX_PATH, "lucene/index");
        string ext = _fileManager.GetFileExtension(name);
        string content = "";

        switch (ext.ToLower())
        {
            case "pdf":
                content = _fileManager.ExtractTextFromPdf(filePath);
                break;
            case "docx":
                content = _fileManager.ExtractTextFromDocx(filePath);
                break;
            case "txt":
                content = _fileManager.ExtractTextFromTxt(filePath);
                break;
        }
        _fileManager.IndexDocument(name, content, luceneIndexPath);
    }
}
