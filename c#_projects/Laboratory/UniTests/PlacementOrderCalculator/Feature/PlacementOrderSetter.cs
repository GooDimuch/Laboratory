using LoadedLions.ConstructionModule;

namespace UniTests;

public interface IPlacementOrderSetter
{
    void ApplyOrders(Vector2Int roomSize,
        IEnumerable<PlacementView> placements,
        Action<PlacementView, int> weightSelector);

    IEnumerable<PlacementView> SortBuildingsByPath(IEnumerable<PlacementView> buildings, int[,] pathMatrix);
    int[,] GenerateMatrix(int columns, int rows);
}

public class PlacementOrderSetter : IPlacementOrderSetter
{
    private readonly IPlacementWeightCalculator _weightCalculator;

    public PlacementOrderSetter(IPlacementWeightCalculator weightCalculator)
    {
        _weightCalculator = weightCalculator;
    }

    public void ApplyOrders(Vector2Int roomSize,
        IEnumerable<PlacementView> placements,
        Action<PlacementView, int> weightSelector)
    {
        var pathMatrix = GenerateMatrix(roomSize.x, roomSize.y);
        var sortedBuildings = SortBuildingsByPath(placements, pathMatrix);
        ApplyWeight(sortedBuildings.ToList(), weightSelector, pathMatrix);
    }

    private void ApplyWeight(IReadOnlyList<PlacementView> sortedPlacements,
        Action<PlacementView, int> weightSelector,
        int[,] pathMatrix)
    {
        var weights = _weightCalculator.CalculateWeight(sortedPlacements, pathMatrix);
        foreach (var placement in sortedPlacements)
        {
            if (!weights.ContainsKey(placement.Data.Id))
                throw new Exception($"Weight for building[{placement.Data.Id} was not found]");
            weightSelector?.Invoke(placement, weights[placement.Data.Id]);
        }
    }

    public IEnumerable<PlacementView> SortBuildingsByPath(IEnumerable<PlacementView> buildings, int[,] pathMatrix) =>
        buildings.OrderBy(placementView => placementView, new PlacementViewComparer(pathMatrix));

    public int[,] GenerateMatrix(int columns, int rows)
    {
        int[,] matrix = new int[rows, columns];
        int value = 1;

        for (int i = 0; i < rows + columns - 1; i++)
            for (int j = 0; j <= i; j++)
                if (j < rows && i - j < columns)
                    matrix[j, i - j] = value++;

        return matrix;
    }
}
