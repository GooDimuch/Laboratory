namespace UniTests;

public static class ShapeConverter
{
    public static int[,] Convert(string areaShape)
    {
        var rows = areaShape.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var area = new int[rows.Length, rows[0].Length];
        for (int y = 0; y < area.GetLength(0); y++)
            for (int x = 0; x < area.GetLength(1); x++)
                area[y, x] = rows.Length > y && rows[y].Length > x && rows[y][x] == '1' ? 1 : 0;
        return area;
    }

    public static int[,] Convert(IEnumerable<Vector3Int> points)
    {
        var size = points.GetSize();

        var area = new int[size.y, size.x];
        foreach (var point in points)
            area[size.y - 1 - point.y, point.x] = 1;

        return area;
    }
}
