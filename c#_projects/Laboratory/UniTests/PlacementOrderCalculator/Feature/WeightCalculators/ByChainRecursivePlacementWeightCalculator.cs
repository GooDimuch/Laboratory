using UniTests;

namespace LoadedLions.ConstructionModule
{
    public class ByChainRecursivePlacementWeightCalculator : IPlacementWeightCalculator
    {
        /// <summary>
        /// result Dictionary with weight by placement id
        /// </summary>
        private readonly Dictionary<long, int> _weights = new Dictionary<long, int>();

        /// <summary>
        /// sorted List placements without weight. Used to iterate over all placements that are not in chains
        /// </summary>
        private readonly List<PlacementView> _withoutWeightPlacements = new List<PlacementView>();

        /// <summary>
        /// use HashSet for avoid StackOverflowException. Save placements chain for mark only unique placements
        /// </summary>
        private readonly HashSet<PlacementView> _markedPlacementsChain = new HashSet<PlacementView>();

        /// <summary>
        /// Dictionary for optimization finding placement by cell
        /// </summary>
        private Dictionary<Vector3Int, PlacementView> _placementCells;

        public Dictionary<long, int> CalculateWeight(IReadOnlyList<PlacementView> placements, int[,] pathMatrix)
        {
            _weights.Clear();
            _withoutWeightPlacements.Clear();
            _withoutWeightPlacements.AddRange(placements);
            _placementCells = GetPlacementCells(placements);

            MarkPlacements();

            return _weights;
        }

        /// <summary>
        /// Cache all used cell for optimization find placement
        /// </summary>
        /// <param name="placements">Sorted all placements in room</param>
        /// <returns>Dictionary with placement by cell</returns>
        private Dictionary<Vector3Int, PlacementView> GetPlacementCells(IEnumerable<PlacementView> placements)
        {
            //todo be careful, PreviewPlacement can overlap cells
            var placementCells = new Dictionary<Vector3Int, PlacementView>();
            foreach (var placement in placements)
                foreach (var cell in placement.Area.allPositionsWithin)
                    if (!placementCells.ContainsKey(cell))
                        placementCells.Add(cell, placement);
                    else
                        placementCells[cell] = placement;
            return placementCells;
        }

        /// <summary>
        /// Calculate weight for all placements
        /// </summary>
        private void MarkPlacements()
        {
            var maxWeight = -1;
            while (_withoutWeightPlacements.Count > 0)
            {
                _markedPlacementsChain.Clear();
                PlacementView firstPlacement = _withoutWeightPlacements[0];
                maxWeight = MarkPlacementRecursive(firstPlacement, maxWeight + 1);
            }
        }

        /// <summary>
        /// Set weight for current placement and calculate weight for upper and right placements.
        /// </summary>
        /// <param name="placement">Placement for set or update weight</param>
        /// <param name="weight">New weight for placement</param>
        /// <returns>MaxWeight for placements chain</returns>
        private int MarkPlacementRecursive(PlacementView placement, int weight)
        {
            SetPlacementWeight(placement, weight);
            _withoutWeightPlacements.Remove(placement);
            _markedPlacementsChain.Add(placement);

            var rightMaxWeight = 0;
            var upperMaxWeight = 0;

            foreach (var cell in placement.Area.allPositionsWithin)
            {
                var currentRightMaxWeight = MarkRightPlacement(cell);
                var currentUpperMaxWeight = MarkUpperPlacement(cell);

                rightMaxWeight = Mathf.Max(currentRightMaxWeight, rightMaxWeight);
                upperMaxWeight = Mathf.Max(currentUpperMaxWeight, upperMaxWeight);
            }

            return Mathf.Max(weight, rightMaxWeight, upperMaxWeight);

            int MarkRightPlacement(Vector3Int cell) =>
                MarkPlacementByDirection(cell, GetRightPlacement);

            int MarkUpperPlacement(Vector3Int cell) =>
                MarkPlacementByDirection(cell, GetUpperPlacement);

            int MarkPlacementByDirection(Vector3Int cell, Func<Vector3Int, PlacementView> getPlacementByDirection)
            {
                var nextPlacement = getPlacementByDirection(cell);
                return nextPlacement != null ? MarkPlacementRecursive(nextPlacement, weight + 1) : 0;
            }

            PlacementView GetUpperPlacement(Vector3Int cell) =>
                GetPlacementByDirection(cell, Vector3Int.up);

            PlacementView GetRightPlacement(Vector3Int cell) =>
                GetPlacementByDirection(cell, Vector3Int.right);

            PlacementView GetPlacementByDirection(Vector3Int cell, Vector3Int direction)
            {
                if (!_placementCells.TryGetValue(cell + direction, out var nextPlacement) ||
                    nextPlacement == placement ||
                    _markedPlacementsChain.Contains(nextPlacement))
                    return null;
                return nextPlacement;
            }
        }

        private void SetPlacementWeight(PlacementView placement, int weight)
        {
            if (!_weights.ContainsKey(placement.Data.Id))
                _weights.Add(placement.Data.Id, weight);
            else
                _weights[placement.Data.Id] = weight;
        }
    }
}
