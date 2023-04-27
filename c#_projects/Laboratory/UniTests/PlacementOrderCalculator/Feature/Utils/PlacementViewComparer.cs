namespace UniTests;

public class PlacementViewComparer : IComparer<PlacementView>
{
    private readonly int[,] _pathMatrix;

    public PlacementViewComparer(int[,] pathMatrix)
    {
        _pathMatrix = pathMatrix;
    }

    public int Compare(PlacementView x, PlacementView y)
    {
        if (x == null || y == null)
            throw new Exception($"[PlacementViewComparer] {nameof(PlacementView)} is null");

        var xPathValue = _pathMatrix[x.Area.Position.y, x.Area.Position.x];
        var yPathValue = _pathMatrix[y.Area.Position.y, y.Area.Position.x];

        return xPathValue.CompareTo(yPathValue);
    }
}
