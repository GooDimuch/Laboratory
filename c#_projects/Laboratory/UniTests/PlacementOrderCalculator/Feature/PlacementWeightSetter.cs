namespace UniTests;

public class PlacementWeightSetter
{
    public Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> sortedPlacements)
    {
        var weights = new Dictionary<long, int>();
        foreach (var placement in sortedPlacements)
        {
            if (!weights.ContainsKey(placement.Data.Id))
                weights.Add(placement.Data.Id, 0);

            foreach (var otherPlacement in sortedPlacements)
            {
                if (otherPlacement == placement)
                    continue;
                if (otherPlacement.IsAboveAndRightOf(placement))
                    IncrementWeight(otherPlacement);
            }
        }
        return weights;

        void IncrementWeight(PlacementView placement)
        {
            if (!weights.ContainsKey(placement.Data.Id))
                weights.Add(placement.Data.Id, 0);
            weights[placement.Data.Id]++;
        }
    }
}

public static class PlacementViewExtension
{
    public static bool IsAboveAndRightOf(this PlacementView other, PlacementView current) =>
        other.Area.allPositionsWithin.Any(cell =>
            cell.x >= current.Area.Position.x && cell.y >= current.Area.Position.y);
}
