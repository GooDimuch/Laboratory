namespace UniTests;

public static class Matrix2dExtension
{
    public static string ToString<T>(this T[,] matrix, string columnSeparator, string rowSeparator)
    {
        var result = "";
        var rows = matrix.GetLength(0);
        for (int i = 0; i < rows; i++)
        {
            result += string.Join(columnSeparator, matrix.GetRow(i));
            if (i + 1 < rows) result += rowSeparator;
        }
        return result;
    }

    public static IEnumerable<T> GetColumn<T>(this T[,] matrix, int columnNumber) =>
        Enumerable.Range(0, matrix.GetLength(0))
            .Select(x => matrix[x, columnNumber]);

    public static IEnumerable<T> GetRow<T>(this T[,] matrix, int rowNumber) =>
        Enumerable.Range(0, matrix.GetLength(1))
            .Select(x => matrix[rowNumber, x]);
}

public static class CollectionExtension
{
    public static string ToString<TSource>(this IEnumerable<TSource> collection,
        string separator,
        Func<TSource, string> to) =>
        string.Join(separator, collection.Select(to));

    public static string ToString<TSource, TKey>(this IEnumerable<TSource> collection,
        string separator,
        Func<TSource, string> to,
        Func<TSource, TKey>? orderBy) =>
        string.Join(separator, orderBy != null ? collection.OrderBy(orderBy).Select(to) : collection.Select(to));

    public static string ToString<K, V>(this IDictionary<K, V> collection,
        string separator,
        Func<KeyValuePair<K, V>, string> to,
        bool sortKeys = false) =>
        string.Join(separator, sortKeys ? collection.OrderBy(pair => pair.Key).Select(to) : collection.Select(to));
}
