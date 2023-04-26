namespace UniTests;

public class PlacementOrderCalculator
{
    private const int WeightCoefficient = 10;

    public void ApplyOrders(Vector3Int roomSize, IEnumerable<PlacementView> buildings)
    {
        var pathMatrix = GenerateMatrix(roomSize.x, roomSize.y);
        var sortedBuildings = SortBuildingsByPath(buildings, pathMatrix);
        ApplyWeight(sortedBuildings.ToList());
    }

    public void ApplyWeight(IReadOnlyList<PlacementView> sortedBuildings)
    {
        var weights = new PlacementWeightSetter().CalculateWeight(sortedBuildings);
        foreach (var building in sortedBuildings)
        {
            if (!weights.ContainsKey(building.Data.Id))
                throw new Exception($"Weight for building[{building.Data.Id} was not found]");
            building.Area.Position.z = weights[building.Data.Id] * WeightCoefficient;
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
