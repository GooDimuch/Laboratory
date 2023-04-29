namespace UniTests;

public class RoomSetuper
{
    public enum Variant
    {
        _2x3_v1,
        _3x2_v1,
        _3x2_v2,
        _4x3_v1,
        _4x3_v2,
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
        _roomData.Add(Variant._4x3_v2, (new Vector2Int(4, 3), Setup_4x3_v2()));
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
            Create( /*B*/2, new Vector3Int(0, 2), "11"),
            Create( /*A*/1, new Vector3Int(1, 0), "1\n1"),
        };

    /////////////
    /// B B . ///
    /// . A A ///
    /////////////
    private IEnumerable<PlacementView> Setup_3x2_v1() =>
        new List<PlacementView>()
        {
            Create( /*B*/2, new Vector3Int(0, 1), "11"),
            Create( /*A*/1, new Vector3Int(1, 0), "11"),
        };

    /////////////
    /// . B B ///
    /// A A . ///
    /////////////
    private IEnumerable<PlacementView> Setup_3x2_v2() =>
        new List<PlacementView>()
        {
            Create( /*B*/2, new Vector3Int(1, 1), "11"),
            Create( /*A*/1, new Vector3Int(0, 0), "11"),
        };

    ///////////////
    /// F F . . ///
    /// . E E E ///
    /// A B C D ///
    ///////////////
    private IEnumerable<PlacementView> Setup_4x3_v1() =>
        new List<PlacementView>()
        {
            Create( /*D*/4, new Vector3Int(3, 0),"1"),
            Create( /*F*/6, new Vector3Int(0, 2),"11"),
            Create( /*E*/5, new Vector3Int(1, 1),"111"),
            Create( /*C*/3, new Vector3Int(2, 0),"1"),
            Create( /*B*/2, new Vector3Int(1, 0),"1"),
            Create( /*A*/1, new Vector3Int(0, 0),"1"),
        };

    ///////////////
    /// . . D D ///
    /// . B B . ///
    /// A A B C ///
    ///////////////
    private IEnumerable<PlacementView> Setup_4x3_v2() =>
        new List<PlacementView>()
        {
            Create( /*D*/4, new Vector3Int(2, 2), "11"),
            Create( /*B*/2, new Vector3Int(1, 0), "11\n01"),
            Create( /*A*/1, new Vector3Int(0, 0), "11"),
            Create( /*C*/3, new Vector3Int(3, 0), "1"),
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
            Create( /*E*/5, new Vector3Int(0, 3), "1\n1"),
            Create( /*J*/10, new Vector3Int(1, 4), "11"),
            Create( /*M*/13, new Vector3Int(3, 4), "11"),
            Create( /*H*/8, new Vector3Int(1, 3), "1"),
            Create( /*I*/9, new Vector3Int(2, 3), "1"),
            Create( /*L*/12, new Vector3Int(3, 3), "1"),
            Create( /*K*/11, new Vector3Int(4, 2), "1\n1"),
            Create( /*B*/2, new Vector3Int(0, 1), "1\n1"),
            Create( /*D*/4, new Vector3Int(1, 2), "1"),
            Create( /*G*/7, new Vector3Int(2, 2), "11"),
            Create( /*C*/3, new Vector3Int(1, 1), "11"),
            Create( /*F*/6, new Vector3Int(3, 1), "11"),
            Create( /*A*/1, new Vector3Int(0, 0), "11111"),
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
            Create( /*E*/5, new Vector3Int(0, 3),"1\n1"),
            Create( /*K*/11, new Vector3Int(1, 4),"11"),
            Create( /*M*/13, new Vector3Int(3, 4),"1"),
            Create( /*F*/6, new Vector3Int(4, 0),"1\n1\n1\n1\n1"),
            Create( /*I*/9, new Vector3Int(1, 3),"1"),
            Create( /*J*/10, new Vector3Int(2, 3),"1"),
            Create( /*L*/12, new Vector3Int(3, 3),"1"),
            Create( /*B*/2, new Vector3Int(0, 1),"1\n1"),
            Create( /*D*/4, new Vector3Int(1, 2),"1"),
            Create( /*H*/8, new Vector3Int(2, 2),"11"),
            Create( /*C*/3, new Vector3Int(1, 1),"11"),
            Create( /*G*/7, new Vector3Int(3, 1),"1"),
            Create( /*A*/1, new Vector3Int(0, 0),"1111"),
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
            Create( /*F*/6, new Vector3Int(0, 6),"1"),
            Create( /*G*/7, new Vector3Int(1, 6),"1"),
            Create( /*H*/8, new Vector3Int(2, 6),"1"),
            Create( /*E*/5, new Vector3Int(0, 5),"1"),
            Create( /*D*/4, new Vector3Int(0, 4),"1"),
            Create( /*A*/1, new Vector3Int(2, 0),"1\n1\n1\n1\n1"),
            Create( /*C*/3, new Vector3Int(0, 3),"1"),
            Create( /*B*/2, new Vector3Int(0, 2),"1"),
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
            Create( /*A*/101, new Vector3Int(0, 0), "11\n11\n11\n11\n11\n11\n11\n11\n11"),
            Create( /*B*/102, new Vector3Int(2, 0), "11111111111111\n11111111111111\n11111111111111\n11111111111111\n1111\n1111\n1111"),
            Create( /*C*/103, new Vector3Int(6, 0),"111\n111"),
            Create( /*D*/104, new Vector3Int(9, 0),"111\n111\n111"),
            Create( /*E*/105, new Vector3Int(12, 0),"111\n111\n111"),
            Create( /*F*/106, new Vector3Int(15, 0),"111\n111"),
            Create( /*G*/107, new Vector3Int(18, 0),"111\n111\n111\n111\n111\n111"),
            Create( /*H*/108, new Vector3Int(21, 0),"111\n111\n111"),
            Create( /*I*/109, new Vector3Int(24, 0),"111\n111\n111"),
            Create( /*J*/110, new Vector3Int(27, 0),"111\n111"),
            Create( /*K*/111, new Vector3Int(10, 12),"111111111\n111111111"),
            Create( /*L*/112, new Vector3Int(10, 14),"111\n111\n111"),
        };

    private PlacementView Create(int id,
        Vector3Int position,
        string shape) =>
        new PlacementView()
        {
            Area = new PlaceArea(shape, position),
            Data = new PlacementData() { Id = id }
        };
}
