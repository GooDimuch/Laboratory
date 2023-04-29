using UniTests;

namespace LoadedLions.ConstructionModule
{
    public class BySizeRecursivePlacementWeightCalculator : IPlacementWeightCalculator
    {
        private const int StackOverflowCalls = 5000;

        private IReadOnlyList<PlacementView> _placements;
        private int[,] _pathMatrix;

        private readonly Dictionary<long, int> _weights = new Dictionary<long, int>();
        private readonly HashSet<PlacementView> _recursiveChain = new HashSet<PlacementView>();

        private int _callCounter;

        public Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> placements, int[,] pathMatrix)
        {
            _pathMatrix = pathMatrix;
            _placements = placements;

            _callCounter = 0;
            _weights.Clear();

            MarkPlacements();

            return _weights;
        }

        private void MarkPlacements()
        {
            if (_placements.Count == 0)
                return;
            foreach (var placement in _placements)
            {
                Console.WriteLine($"Chain length {_recursiveChain.Count}");
                MarkPlacementRecursive(placement, GetPlacementWeight(placement));
            }
        }

        private void MarkPlacementRecursive(PlacementView placement, int weight)
        {
            if (_callCounter++ > StackOverflowCalls)
                throw new StackOverflowException(
                    $"[Order] {nameof(BySizeRecursivePlacementWeightCalculator)}.{nameof(MarkPlacementRecursive)} " +
                    $"has more {StackOverflowCalls} calls. Placements {string.Join("\n", _placements.Select(p => $"{p}"))}");

            var id = GetId(placement);
            _recursiveChain.Add(placement);
            SetPlacementWeight(placement, weight);

            foreach (var otherPlacement in _placements)
            {
                var otherId = GetId(otherPlacement);
                if (placement == otherPlacement ||
                    _recursiveChain.Contains(otherPlacement) ||
                    !otherPlacement.IsAboveAndRightOf(placement) ||
                    GetPlacementWeight(otherPlacement) > weight)
                    continue;
                MarkPlacementRecursive(otherPlacement, weight + 1);
            }

            _recursiveChain.Remove(placement);
        }

        private void SetPlacementWeight(PlacementView placement, int weight)
        {
            Console.WriteLine($"Set id[{GetId(placement)}]: {weight}");
            if (!_weights.ContainsKey(placement.Data.Id))
                _weights.Add(placement.Data.Id, weight);
            else
                _weights[placement.Data.Id] = weight;
        }

        private int GetPlacementWeight(PlacementView placement) =>
            _weights.ContainsKey(placement.Data.Id) ? _weights[placement.Data.Id] : 0;

        private char GetId(PlacementView placement) =>
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)placement.Data.Id - 1];
    }
}
