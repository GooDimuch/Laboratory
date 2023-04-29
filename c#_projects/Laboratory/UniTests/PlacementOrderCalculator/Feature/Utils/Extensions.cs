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

    public static Vector2Int GetSize(this IEnumerable<Vector3Int> points)
    {
        var maxX = 0;
        var maxY = 0;

        foreach (var point in points)
        {
            if (point.x > maxX)
                maxX = point.x;
            if (point.y > maxY)
                maxY = point.y;
        }

        return new Vector2Int(maxX, maxY) + Vector2Int.one;
    }
}

public static class PlacementViewExtension
{
    public static bool IsAboveAndRightOf(this PlacementView other, PlacementView current)
    {
        return IsDefaultCondition()
            ? OtherRightTopAboveAndRightOfCurrentLeftBottom()
            : OtherRightTopAboveAndRightOfCurrentRightTop();

        bool IsDefaultCondition() =>
            IsConvex(current) || !OtherLeftBottomBelowAndLeftOfCurrentLeftBottom();

        bool IsConvex(PlacementView placement) =>
            placement.Area.Area.Cast<int>().All(i => i == 1);

        bool OtherRightTopAboveAndRightOfCurrentLeftBottom() =>
            other.Area.Position.x + other.Area.size.x - 1 >= current.Area.Position.x &&
            other.Area.Position.y + other.Area.size.y - 1 >= current.Area.Position.y;

        bool OtherLeftBottomBelowAndLeftOfCurrentLeftBottom() =>
            other.Area.Position.x + other.Area.size.x - 1 <= current.Area.Position.x &&
            other.Area.Position.y + other.Area.size.y - 1 <= current.Area.Position.y;

        bool OtherRightTopAboveAndRightOfCurrentRightTop() =>
            other.Area.Position.x + other.Area.size.x - 1 >= current.Area.Position.x + current.Area.size.x - 1 &&
            other.Area.Position.y + other.Area.size.y - 1 >= current.Area.Position.y + current.Area.size.y - 1;
    }
}

public static class AreaExtension
{
    //todo need to optimize matrix rotation
    public static int[,] Rotate(this int[,] areaRotate0, OrientationType orientation) =>
        orientation switch
        {
            OrientationType.Rotate0 => (int[,])areaRotate0.Clone(),
            OrientationType.Rotate90 => RotateMatrixCounterClockwise(
                RotateMatrixCounterClockwise(RotateMatrixCounterClockwise(areaRotate0))),
            OrientationType.Rotate180 =>
                RotateMatrixCounterClockwise(RotateMatrixCounterClockwise(areaRotate0)),
            OrientationType.Rotate270 => RotateMatrixCounterClockwise(areaRotate0),
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };

    public static int[,] RotateMatrixCounterClockwise(int[,] oldMatrix)
    {
        int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }
}
