namespace CustomerChargeNotification.Services;

/// <summary>
/// This extention a helper to create batch of 
/// defined batchSize from IEnumerable
/// </summary>
public static class Extentions
{
    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        var batch = new List<T>(batchSize);
        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }
        if (batch.Count > 0)
            yield return batch;
    }
}
