namespace UniTests
{
    public enum OrientationType : byte
    {
        Rotate0 = 0,
        Rotate90 = 1,
        Rotate180 = 2,
        Rotate270 = 3
    }

    public class PlaceArea : ICloneable
    {
        private string _areaShape;

        private Vector3Int _position;

        public Vector3Int Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                UpdatePosition();
            }
        }

        private OrientationType _orientation;

        public OrientationType Orientation
        {
            get => _orientation;
            set
            {
                if (_orientation == value)
                    return;
                _orientation = value;
                UpdatePlaceArea();
            }
        }

        public Vector3Int pivot;
        public Vector3Int pivotPosition;
        public Vector3Int size;
        public List<Vector3Int> allPositionsWithin;

        private int[,] _area;

        public int[,] Area
        {
            get => _area ??= ShapeConverter.Convert(_areaShape);
            private set
            {
                _area = value;
                UpdatePlaceArea();
            }
        }

        private Dictionary<OrientationType, int[,]> _rotatedAreas;

        public PlaceArea(int[,] area, Vector3Int position, OrientationType orientation)
        {
            _areaShape = AreaToString(area);
            _rotatedAreas = GetAreaRotates(area);
            _position = position;
            _orientation = orientation;
            Area = _rotatedAreas[orientation];
        }

        public PlaceArea(string areaShape, Vector3Int position, OrientationType orientation = OrientationType.Rotate0)
            : this(ShapeConverter.Convert(areaShape), position, orientation) { }

        public PlaceArea(IEnumerable<Vector3Int> points,
            Vector3Int position,
            OrientationType orientation = OrientationType.Rotate0) : this(
            ShapeConverter.Convert(points),
            position,
            orientation) { }

        public void OnValidate()
        {
            Area = ShapeConverter.Convert(_areaShape);
            _rotatedAreas = GetAreaRotates(Area);
        }

        public static Vector3Int GetPivotPosition(Vector3Int position, IEnumerable<Vector3Int> points) =>
            position + GetPivot(ShapeConverter.Convert(points));

        public void Rotate(int direction)
        {
            var orientation = (int)Orientation + direction;
            var directions = Enum.GetValues(typeof(OrientationType)).Length;
            _orientation = (OrientationType)(orientation > directions - 1 ? orientation - directions :
                orientation < 0 ? orientation + directions : orientation);
            Area = _rotatedAreas[Orientation];
        }

        public int AmountOfCells()
        {
            var cells = 0;
            for (int i = 0; i < Area.GetLength(0); i++)
                for (int j = 0; j < Area.GetLength(1); j++)
                    if (Area[i, j] == 1)
                        cells++;
            return cells;
        }

        private void UpdatePlaceArea()
        {
            pivot = GetPivot(Area);
            size = GetSize(Area);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            pivotPosition = _position + pivot;
            allPositionsWithin = GetAllPositionsWithin(Area, _position);
        }

        private static Vector3Int GetPivot(int[,] area)
        {
            var offset = Vector3Int.zero;
            for (int i = 0; i < area.GetLength(1); i++)
                if (area[area.GetLength(0) - 1, i] == 1)
                    break;
                else
                    offset += Vector3Int.right;
            return offset;
        }

        private Vector3Int GetSize(int[,] area) =>
            new Vector3Int(area.GetLength(1), area.GetLength(0), 1);

        private List<Vector3Int> GetAllPositionsWithin(int[,] area, Vector3Int position)
        {
            var tilePositions = new List<Vector3Int>();

            var rows = area.GetLength(0);
            var columns = area.GetLength(1);

            for (int i = rows - 1; i >= 0; i--)
                for (int j = 0; j < columns; j++)
                    if (area[i, j] == 1)
                        tilePositions.Add(position + new Vector3Int(j, rows - 1 - i, 0));
            return tilePositions;
        }

        private Dictionary<OrientationType, int[,]> GetAreaRotates(int[,] area) =>
            new Dictionary<OrientationType, int[,]>
            {
                { OrientationType.Rotate0, area.Rotate(OrientationType.Rotate0) },
                { OrientationType.Rotate90, area.Rotate(OrientationType.Rotate90) },
                { OrientationType.Rotate180, area.Rotate(OrientationType.Rotate180) },
                { OrientationType.Rotate270, area.Rotate(OrientationType.Rotate270) }
            };

        private string AreaToString(int[,] area)
        {
            var shape = "";
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(1); j++)
                    shape += area[i, j];
                if (i != area.GetLength(0) - 1)
                    shape += Environment.NewLine;
            }
            return shape;
        }

        public object Clone() =>
            new PlaceArea(_rotatedAreas[OrientationType.Rotate0], _position, _orientation);

        public override string ToString() =>
            $"{nameof(PlaceArea)}: Position = '{Position}', Orientation = '{Orientation}', pivotPosition = '{pivotPosition}'\n" +
            $"Shape: \n'{AreaToString(Area)}'";
    }
}
