using UniTests;

namespace LoadedLions.ConstructionModule
{
    public class BySizeRecursivePlacementWeightCalculator : IPlacementWeightCalculator
    {
        private IReadOnlyList<PlacementView> _placements;
        private int[,] _pathMatrix;

        private readonly Dictionary<long, int> _weights = new();

        public Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> placements, int[,] pathMatrix)
        {
            _pathMatrix = pathMatrix;
            _placements = placements;

            _weights.Clear();

            MarkPlacements();

            return _weights;
        }

        private void MarkPlacements()
        {
            if (_placements.Count == 0)
                return;
            foreach (var placement in _placements)
                MarkPlacementRecursive(placement, GetPlacementWeight(placement));
        }

        private void MarkPlacementRecursive(PlacementView placement, int weight)
        {
            SetPlacementWeight(placement, weight);

            foreach (var otherPlacement in _placements)
            {
                if (placement == otherPlacement)
                    continue;
                if (otherPlacement.IsAboveAndRightOf(placement))
                    MarkPlacementRecursive(otherPlacement, weight + 1);
            }
        }

        private void SetPlacementWeight(PlacementView placement, int weight)
        {
            var ids = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Console.WriteLine($"Set id[{ids[(int)placement.Data.Id - 1]}]: {weight}");
            if (!_weights.ContainsKey(placement.Data.Id))
                _weights.Add(placement.Data.Id, weight);
            else
                _weights[placement.Data.Id] = weight;
        }

        private int GetPlacementWeight(PlacementView placement) =>
            _weights.ContainsKey(placement.Data.Id) ? _weights[placement.Data.Id] : 0;
    }
}
