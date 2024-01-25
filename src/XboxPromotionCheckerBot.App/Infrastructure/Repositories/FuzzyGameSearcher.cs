using System.Runtime.CompilerServices;
using Akade.IndexedSet;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Extensions;

namespace XboxPromotionCheckerBot.App.Infrastructure.Repositories;

public sealed record FuzzGame(Guid Id, string Title)
{
    public FuzzGame(string Title): this(Guid.NewGuid(), Title)
    {

    }
}

public sealed class FuzzyGameSearcher : IGameSearcher
{
    const LuceneVersion LuceneVersion = Lucene.Net.Util.LuceneVersion.LUCENE_48;
    private readonly Analyzer _analyzer;
    private readonly FuzzGame[] _gamesIWant;
    private readonly FSDirectory _fsDirectory;
    private const string TitleFieldName = "title";
    private FuzzyGameSearcher(Analyzer analyzer, FSDirectory fsDirectory, FuzzGame[] gamesIWant)
    {
        _analyzer = analyzer;
        _fsDirectory = fsDirectory;
        _gamesIWant = gamesIWant;
    }


    public async IAsyncEnumerable<XboxGame> FilterExistingGames(IAsyncEnumerable<XboxGame> games, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IndexWriterConfig indexConfig = new IndexWriterConfig(LuceneVersion, _analyzer)
        {
            OpenMode = OpenMode.CREATE
        };
        
        using var writer = new IndexWriter(_fsDirectory, indexConfig);
        
        foreach (var gameIWant in _gamesIWant)   
        {
            var document = new Document { new TextField(TitleFieldName, gameIWant.Title, Field.Store.YES) };
            writer.AddDocument(document);
        }
        writer.Commit();
        
        using var reader = writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            var queryParser = new QueryParser(LuceneVersion.LUCENE_48, TitleFieldName, _analyzer);
            var query = queryParser.Parse(game.Title);
            
            var exists = searcher.Search(query, 1);

            if (exists?.ScoreDocs is {Length: >0})
            {
                yield return game;
            }
        }
    }

    public static FuzzyGameSearcher Create(IEnumerable<FuzzGame> games)
    {
        const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
        Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);
        const string indexName = "games";
        var indexPath = Path.Combine(Environment.CurrentDirectory, indexName);
        var indexDir = FSDirectory.Open(indexPath);
        return new FuzzyGameSearcher(standardAnalyzer, indexDir, games.ToArray());
    }

    public void Dispose()
    {
        _analyzer.Dispose();
        _fsDirectory.Dispose();
    }
}