using System;
using DocumentFormat.OpenXml.Packaging;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using sylas_api.Database;
using UglyToad.PdfPig;

namespace sylas_api.JobManagers;

public class FileManager(SyContext context) : SyManager(context)
{
    private const LuceneVersion VERSION = LuceneVersion.LUCENE_48;
    private readonly string _indexPath = "lucene_index";
    public void IndexDocument(string title, string content)
    {
        var dir = FSDirectory.Open(_indexPath);
        var analyzer = new StandardAnalyzer(VERSION);
        var config = new IndexWriterConfig(VERSION, analyzer);
        using (var writer = new IndexWriter(dir, config))
        {
            var doc = new Document
            {
                new TextField("title", title, Field.Store.YES),
                new TextField("content", content, Field.Store.YES)
            };
            writer.AddDocument(doc);
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }
    }

    public List<string> SearchDocuments(string queryText)
    {
        var dir = FSDirectory.Open(_indexPath);
        var analyzer = new StandardAnalyzer(VERSION);
        using var reader = DirectoryReader.Open(dir);
        var searcher = new IndexSearcher(reader);
        var parser = new MultiFieldQueryParser(VERSION, new[] { "title", "content" }, analyzer);
        var query = parser.Parse(queryText);

        var hits = searcher.Search(query, 10);

        return [.. hits.ScoreDocs.Select(h => searcher.Doc(h.Doc).Get("title"))];
    }

    public string ExtractTextFromPdf(string filePath)
    {
        using var doc = PdfDocument.Open(filePath);
        return string.Join("\n", doc.GetPages().Select(p => p.Text));
    }

    public string ExtractTextFromDocx(string filePath)
    {
        using var doc = WordprocessingDocument.Open(filePath, false);
        return doc.MainDocumentPart?.Document.Body?.InnerText ?? "";
    }
}
