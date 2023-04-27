using UniTests;

namespace LoadedLions.ConstructionModule
{
    public class PlacementWeightByPathCalculator : IPlacementWeightCalculator
    {
        public Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> sortedPlacements, int[,] pathMatrix)
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
}
