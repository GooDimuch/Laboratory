using UniTests;

namespace LoadedLions.ConstructionModule
{
    public interface IPlacementWeightCalculator
    {
        Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> placements, int[,] pathMatrix);
    }
}
