using Microsoft.Extensions.VectorData;
namespace RAG_Shared;

public class Movie<T>
{
    [VectorStoreRecordKey]
    public T Key { get; set; }
    
    [VectorStoreRecordData]
    public string? Title { get; set; }
    
    [VectorStoreRecordData]
    public int? Year { get; set; }
    
    [VectorStoreRecordData]
    public string? Category { get; set; }
    
    [VectorStoreRecordData]
    public string? Description { get; set; }
}

public class MovieVector<T> : Movie<T>
{
    [VectorStoreRecordVector(384, DistanceFunction.DotProductSimilarity)]
    public ReadOnlyMemory<float> Vector { get; set; }
}

public class MovieSqlite<T> : Movie<T>
{
    [VectorStoreRecordVector(4, DistanceFunction.DotProductSimilarity)]
    public ReadOnlyMemory<float> DescriptionEmbedding { get; set; }
}