namespace UniTests;

public class RoomSetuper
{
    public enum Variant
    {
        _2x3_v1,
        _3x2_v1,
        _3x2_v2,
        _4x3_v1,
        _5x5_v1,
        _5x5_v2,
        _5x9_v1,
        _33x48_v1,
    }

    private readonly Dictionary<Variant, (Vector2Int size, IEnumerable<PlacementView> placements)>
        _roomData = new();

    public void Setup()
    {
        _roomData.Add(Variant._2x3_v1, (new Vector2Int(2, 3), Setup_2x3_v1()));
        _roomData.Add(Variant._3x2_v1, (new Vector2Int(3, 2), Setup_3x2_v1()));
        _roomData.Add(Variant._3x2_v2, (new Vector2Int(3, 2), Setup_3x2_v2()));
        _roomData.Add(Variant._4x3_v1, (new Vector2Int(4, 3), Setup_4x3_v1()));
        _roomData.Add(Variant._5x5_v1, (new Vector2Int(5, 5), Setup_5x5_v1()));
        _roomData.Add(Variant._5x5_v2, (new Vector2Int(5, 5), Setup_5x5_v2()));
        _roomData.Add(Variant._5x9_v1, (new Vector2Int(5, 9), Setup_5x9_v1()));
        _roomData.Add(Variant._33x48_v1, (new Vector2Int(33, 48), Setup_33x48_v1()));
    }

    public (Vector2Int size, IEnumerable<PlacementView> placements) Get(Variant variant)
    {
        if (!_roomData.ContainsKey(variant))
            throw new Exception($"RoomSetuper has not {variant}");
        return _roomData[variant];
    }

    ///////////
    /// B B ///
    /// . A ///
    /// . A ///
    ///////////
    private IEnumerable<PlacementView> Setup_2x3_v1() =>
        new List<PlacementView>()
        {
            Create( /*B*/2, new Vector3Int(0, 2), new Vector3Int(2, 1), new Vector3Int(0, 2), new Vector3Int(1, 2)),
            Create( /*A*/1, new Vector3Int(1, 0), new Vector3Int(1, 2), new Vector3Int(1, 0), new Vector3Int(1, 1)),
        };

    /////////////
    /// B B . ///
    /// . A A ///
    /////////////
    private IEnumerable<PlacementView> Setup_3x2_v1() =>
        new List<PlacementView>()
        {
            Create( /*B*/2, new Vector3Int(0, 1), new Vector3Int(2, 1), new Vector3Int(0, 1), new Vector3Int(1, 1)),
            Create( /*A*/1, new Vector3Int(1, 0), new Vector3Int(2, 1), new Vector3Int(1, 0), new Vector3Int(2, 0)),
        };

    /////////////
    /// . B B ///
    /// A A . ///
    /////////////
    private IEnumerable<PlacementView> Setup_3x2_v2() =>
        new List<PlacementView>()
        {
            Create( /*B*/2, new Vector3Int(1, 1), new Vector3Int(2, 1), new Vector3Int(1, 1), new Vector3Int(2, 1)),
            Create( /*A*/1, new Vector3Int(0, 0), new Vector3Int(2, 1), new Vector3Int(0, 0), new Vector3Int(1, 0)),
        };

    ///////////////
    /// F F . . ///
    /// . E E E ///
    /// A B C D ///
    ///////////////
    private IEnumerable<PlacementView> Setup_4x3_v1() =>
        new List<PlacementView>()
        {
            Create( /*D*/4, new Vector3Int(3, 0), new Vector3Int(1, 1), new Vector3Int(3, 0)),
            Create( /*F*/6, new Vector3Int(0, 2), new Vector3Int(2, 1), new Vector3Int(0, 2), new Vector3Int(1, 2)),
            Create( /*E*/5, new Vector3Int(1, 1), new Vector3Int(3, 1), new Vector3Int(1, 1), new Vector3Int(2, 1),
                new Vector3Int(3, 1)),
            Create( /*C*/3, new Vector3Int(2, 0), new Vector3Int(1, 1), new Vector3Int(2, 0)),
            Create( /*B*/2, new Vector3Int(1, 0), new Vector3Int(1, 1), new Vector3Int(1, 0)),
            Create( /*A*/1, new Vector3Int(0, 0), new Vector3Int(1, 1), new Vector3Int(0, 0)),
        };

    /////////////////
    /// E J J M M ///
    /// E H I L K ///
    /// B D G G K ///
    /// B C C F F ///
    /// A A A A A ///
    /////////////////
    private IEnumerable<PlacementView> Setup_5x5_v1() =>
        new List<PlacementView>()
        {
            Create( /*E*/5, new Vector3Int(0, 3), new Vector3Int(1, 2), new Vector3Int(0, 3), new Vector3Int(0, 4)),
            Create( /*J*/10, new Vector3Int(1, 4), new Vector3Int(2, 1), new Vector3Int(1, 4), new Vector3Int(2, 4)),
            Create( /*M*/13, new Vector3Int(3, 4), new Vector3Int(2, 1), new Vector3Int(3, 4), new Vector3Int(4, 4)),
            Create( /*H*/8, new Vector3Int(1, 3), new Vector3Int(1, 1), new Vector3Int(1, 3)),
            Create( /*I*/9, new Vector3Int(2, 3), new Vector3Int(1, 1), new Vector3Int(2, 3)),
            Create( /*L*/12, new Vector3Int(3, 3), new Vector3Int(1, 1), new Vector3Int(3, 3)),
            Create( /*K*/11, new Vector3Int(4, 2), new Vector3Int(1, 2), new Vector3Int(4, 2), new Vector3Int(4, 3)),
            Create( /*B*/2, new Vector3Int(0, 1), new Vector3Int(1, 2), new Vector3Int(0, 1), new Vector3Int(0, 2)),
            Create( /*D*/4, new Vector3Int(1, 2), new Vector3Int(1, 1), new Vector3Int(1, 2)),
            Create( /*G*/7, new Vector3Int(2, 2), new Vector3Int(2, 1), new Vector3Int(2, 2), new Vector3Int(3, 2)),
            Create( /*C*/3, new Vector3Int(1, 1), new Vector3Int(2, 1), new Vector3Int(1, 1), new Vector3Int(2, 1)),
            Create( /*F*/6, new Vector3Int(3, 1), new Vector3Int(2, 1), new Vector3Int(3, 1), new Vector3Int(4, 1)),
            Create( /*A*/1, new Vector3Int(0, 0), new Vector3Int(5, 1), new Vector3Int(0, 0), new Vector3Int(1, 0),
                new Vector3Int(2, 0), new Vector3Int(3, 0), new Vector3Int(4, 0)),
        };

    /////////////////
    /// E K K M F ///
    /// E I J L F ///
    /// B D H H F ///
    /// B C C G F ///
    /// A A A A F ///
    /////////////////
    private IEnumerable<PlacementView> Setup_5x5_v2() =>
        new List<PlacementView>()
        {
            Create( /*E*/5, new Vector3Int(0, 3), new Vector3Int(1, 2), new Vector3Int(0, 3), new Vector3Int(0, 4)),
            Create( /*K*/11, new Vector3Int(1, 4), new Vector3Int(2, 1), new Vector3Int(1, 4), new Vector3Int(2, 4)),
            Create( /*M*/13, new Vector3Int(3, 4), new Vector3Int(1, 1), new Vector3Int(3, 4)),
            Create( /*F*/6, new Vector3Int(4, 0), new Vector3Int(1, 5), new Vector3Int(4, 0), new Vector3Int(4, 1),
                new Vector3Int(4, 2), new Vector3Int(4, 3), new Vector3Int(4, 4)),
            Create( /*I*/9, new Vector3Int(1, 3), new Vector3Int(1, 1), new Vector3Int(1, 3)),
            Create( /*J*/10, new Vector3Int(2, 3), new Vector3Int(1, 1), new Vector3Int(2, 3)),
            Create( /*L*/12, new Vector3Int(3, 3), new Vector3Int(1, 1), new Vector3Int(3, 3)),
            Create( /*B*/2, new Vector3Int(0, 1), new Vector3Int(1, 2), new Vector3Int(0, 1), new Vector3Int(0, 2)),
            Create( /*D*/4, new Vector3Int(1, 2), new Vector3Int(1, 1), new Vector3Int(1, 2)),
            Create( /*H*/8, new Vector3Int(2, 2), new Vector3Int(2, 1), new Vector3Int(2, 2), new Vector3Int(3, 2)),
            Create( /*C*/3, new Vector3Int(1, 1), new Vector3Int(2, 1), new Vector3Int(1, 1), new Vector3Int(2, 1)),
            Create( /*G*/7, new Vector3Int(3, 1), new Vector3Int(1, 1), new Vector3Int(3, 1)),
            Create( /*A*/1, new Vector3Int(0, 0), new Vector3Int(4, 1), new Vector3Int(0, 0), new Vector3Int(1, 0),
                new Vector3Int(2, 0), new Vector3Int(3, 0)),
        };

    /////////////////
    /// . . . . . ///
    /// . . . . . ///
    /// F G H . . ///
    /// E . . . . ///
    /// D . A . . ///
    /// C . A . . ///
    /// B . A . . ///
    /// . . A . . ///
    /// . . A . . ///
    /////////////////
    private IEnumerable<PlacementView> Setup_5x9_v1() =>
        new List<PlacementView>()
        {
            Create( /*F*/6, new Vector3Int(0, 6), new Vector3Int(1, 1), new Vector3Int(0, 6)),
            Create( /*G*/7, new Vector3Int(1, 6), new Vector3Int(1, 1), new Vector3Int(1, 6)),
            Create( /*H*/8, new Vector3Int(2, 6), new Vector3Int(1, 1), new Vector3Int(2, 6)),
            Create( /*E*/5, new Vector3Int(0, 5), new Vector3Int(1, 1), new Vector3Int(0, 5)),
            Create( /*D*/4, new Vector3Int(0, 4), new Vector3Int(1, 1), new Vector3Int(0, 4)),
            Create( /*A*/1, new Vector3Int(2, 0), new Vector3Int(1, 5), new Vector3Int(2, 0),
                new Vector3Int(2, 1), new Vector3Int(2, 2), new Vector3Int(2, 3), new Vector3Int(2, 4)),
            Create( /*C*/3, new Vector3Int(0, 3), new Vector3Int(1, 1), new Vector3Int(0, 3)),
            Create( /*B*/2, new Vector3Int(0, 2), new Vector3Int(1, 1), new Vector3Int(0, 2)),
        };

    /////////////////////////////////////////////////////////////////////
    /// . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . L L L . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . L L L . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . L L L . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . K K K K K K K K K . . . . . . . . . . . . ///
    /// . . . . . . . . . . K K K K K K K K K . . . . . . . . . . . . ///
    /// . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// A A . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// A A . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ///
    /// A A B B B B B B B B B B B B B B . . . . . . . . . . . . . . . ///
    /// A A B B B B B B B B B B B B B B . . G G G . . . . . . . . . . ///
    /// A A B B B B B B B B B B B B B B . . G G G . . . . . . . . . . ///
    /// A A B B B B B B B B B B B B B B . . G G G . . . . . . . . . . ///
    /// A A B B B B . . . D D D E E E . . . G G G H H H I I I . . . . ///
    /// A A B B B B C C C D D D E E E F F F G G G H H H I I I J J J . ///
    /// A A B B B B C C C D D D E E E F F F G G G H H H I I I J J J . ///
    /////////////////////////////////////////////////////////////////////
    private IEnumerable<PlacementView> Setup_33x48_v1() =>
        new List<PlacementView>()
        {
            Create( /*A*/101, new Vector3Int(0, 0),
                new Vector3Int(0, 0), new Vector3Int(0, 1), new Vector3Int(0, 2), new Vector3Int(0, 3),
                new Vector3Int(0, 4), new Vector3Int(0, 5), new Vector3Int(0, 6), new Vector3Int(0, 7),
                new Vector3Int(0, 8),
                new Vector3Int(1, 0), new Vector3Int(1, 1), new Vector3Int(1, 2), new Vector3Int(1, 3),
                new Vector3Int(1, 4), new Vector3Int(1, 5), new Vector3Int(1, 6), new Vector3Int(1, 7),
                new Vector3Int(1, 8)),
            Create( /*B*/102, new Vector3Int(2, 0),
                new Vector3Int(2, 0), new Vector3Int(2, 1), new Vector3Int(2, 2), new Vector3Int(2, 3),
                new Vector3Int(2, 4), new Vector3Int(2, 5), new Vector3Int(2, 6),
                new Vector3Int(3, 0), new Vector3Int(3, 1), new Vector3Int(3, 2), new Vector3Int(3, 3),
                new Vector3Int(3, 4), new Vector3Int(3, 5), new Vector3Int(3, 6),
                new Vector3Int(4, 0), new Vector3Int(4, 1), new Vector3Int(4, 2), new Vector3Int(4, 3),
                new Vector3Int(4, 4), new Vector3Int(4, 5), new Vector3Int(4, 6),
                new Vector3Int(5, 0), new Vector3Int(5, 1), new Vector3Int(5, 2), new Vector3Int(5, 3),
                new Vector3Int(5, 4), new Vector3Int(5, 5), new Vector3Int(5, 6),
                new Vector3Int(6, 0), new Vector3Int(6, 1), new Vector3Int(6, 2), new Vector3Int(6, 3),
                new Vector3Int(7, 0), new Vector3Int(7, 1), new Vector3Int(7, 2), new Vector3Int(7, 3),
                new Vector3Int(8, 0), new Vector3Int(8, 1), new Vector3Int(8, 2), new Vector3Int(8, 3),
                new Vector3Int(9, 0), new Vector3Int(9, 1), new Vector3Int(9, 2), new Vector3Int(9, 3),
                new Vector3Int(10, 0), new Vector3Int(10, 1), new Vector3Int(10, 2), new Vector3Int(10, 3),
                new Vector3Int(11, 0), new Vector3Int(11, 1), new Vector3Int(11, 2), new Vector3Int(11, 3),
                new Vector3Int(12, 0), new Vector3Int(12, 1), new Vector3Int(12, 2), new Vector3Int(12, 3),
                new Vector3Int(13, 0), new Vector3Int(13, 1), new Vector3Int(13, 2), new Vector3Int(13, 3),
                new Vector3Int(14, 0), new Vector3Int(14, 1), new Vector3Int(14, 2), new Vector3Int(14, 3),
                new Vector3Int(15, 0), new Vector3Int(15, 1), new Vector3Int(15, 2), new Vector3Int(15, 3)),
            Create( /*C*/103, new Vector3Int(6, 0),
                new Vector3Int(6, 0), new Vector3Int(6, 1),
                new Vector3Int(7, 0), new Vector3Int(7, 1),
                new Vector3Int(8, 0), new Vector3Int(8, 1)),
            Create( /*D*/104, new Vector3Int(9, 0),
                new Vector3Int(9, 0), new Vector3Int(9, 1), new Vector3Int(9, 2),
                new Vector3Int(10, 0), new Vector3Int(10, 1), new Vector3Int(10, 2),
                new Vector3Int(11, 0), new Vector3Int(11, 1), new Vector3Int(11, 2)),
            Create( /*E*/105, new Vector3Int(12, 0),
                new Vector3Int(12, 0), new Vector3Int(12, 1), new Vector3Int(12, 2),
                new Vector3Int(13, 0), new Vector3Int(13, 1), new Vector3Int(13, 2),
                new Vector3Int(14, 0), new Vector3Int(14, 1), new Vector3Int(14, 2)),
            Create( /*F*/106, new Vector3Int(15, 0),
                new Vector3Int(15, 0), new Vector3Int(15, 1),
                new Vector3Int(16, 0), new Vector3Int(16, 1),
                new Vector3Int(17, 0), new Vector3Int(17, 1)),
            Create( /*G*/107, new Vector3Int(18, 0),
                new Vector3Int(18, 0), new Vector3Int(18, 1), new Vector3Int(18, 2), new Vector3Int(18, 3),
                new Vector3Int(18, 4), new Vector3Int(18, 5),
                new Vector3Int(19, 0), new Vector3Int(19, 1), new Vector3Int(19, 2), new Vector3Int(19, 3),
                new Vector3Int(19, 4), new Vector3Int(19, 5),
                new Vector3Int(20, 0), new Vector3Int(20, 1), new Vector3Int(20, 2), new Vector3Int(20, 3),
                new Vector3Int(20, 4), new Vector3Int(20, 5)),
            Create( /*H*/108, new Vector3Int(21, 0),
                new Vector3Int(21, 0), new Vector3Int(21, 1), new Vector3Int(21, 2),
                new Vector3Int(22, 0), new Vector3Int(22, 1), new Vector3Int(22, 2),
                new Vector3Int(23, 0), new Vector3Int(23, 1), new Vector3Int(23, 2)),
            Create( /*I*/109, new Vector3Int(24, 0),
                new Vector3Int(24, 0), new Vector3Int(24, 1), new Vector3Int(24, 2),
                new Vector3Int(25, 0), new Vector3Int(25, 1), new Vector3Int(25, 2),
                new Vector3Int(26, 0), new Vector3Int(26, 1), new Vector3Int(26, 2)),
            Create( /*J*/110, new Vector3Int(27, 0),
                new Vector3Int(27, 0), new Vector3Int(27, 1),
                new Vector3Int(28, 0), new Vector3Int(28, 1),
                new Vector3Int(29, 0), new Vector3Int(29, 1)),
            Create( /*K*/111, new Vector3Int(10, 12),
                new Vector3Int(10, 12), new Vector3Int(11, 12), new Vector3Int(12, 12), new Vector3Int(13, 12),
                new Vector3Int(14, 12), new Vector3Int(15, 12), new Vector3Int(16, 12), new Vector3Int(17, 12),
                new Vector3Int(18, 12),
                new Vector3Int(10, 13), new Vector3Int(11, 13), new Vector3Int(12, 13), new Vector3Int(13, 13),
                new Vector3Int(14, 13), new Vector3Int(15, 13), new Vector3Int(16, 13), new Vector3Int(17, 13),
                new Vector3Int(18, 13)),
            Create( /*L*/112, new Vector3Int(10, 14),
                new Vector3Int(10, 14), new Vector3Int(10, 15), new Vector3Int(10, 16),
                new Vector3Int(11, 14), new Vector3Int(11, 15), new Vector3Int(11, 16),
                new Vector3Int(12, 14), new Vector3Int(12, 15), new Vector3Int(12, 16)),
        };

    private PlacementView Create(int id,
        Vector3Int position,
        Vector3Int size,
        params Vector3Int[] allPositionsWithin) =>
        new PlacementView()
        {
            Area = new PlaceArea()
            {
                Position = position,
                size = new Vector3Int(size.x, size.y, 1),
                allPositionsWithin = allPositionsWithin.ToList()
            },
            Data = new PlacementData() { Id = id }
        };
}
