namespace UniTests;

public class PlacementOrderCalculatorTests
{
    private PlacementOrderCalculator _orderCalculator;
    private RoomSetuper _roomSetuper;

    [SetUp]
    public void Setup()
    {
        _orderCalculator = new PlacementOrderCalculator();
        _roomSetuper = new RoomSetuper();
        _roomSetuper.Setup();
    }

    [Test]
    [TestCase(RoomSetuper.Variant._3x2_v1, "1:0,2:10")]
    [TestCase(RoomSetuper.Variant._3x2_v2, "1:0,2:10")]
    [TestCase(RoomSetuper.Variant._5x5_v1, "1:0,2:10,3:20,4:30,5:20,6:30,7:50,8:50,9:70,10:80,11:100,12:90,13:120")]
    [TestCase(RoomSetuper.Variant._5x5_v2, "1:0,2:10,3:20,4:30,5:20,6:120,7:30,8:50,9:50,10:70,11:80,12:90,13:110")]
    [TestCase(RoomSetuper.Variant._33x48_v1,
        "101:0,102:50,103:20,104:30,105:40,106:50,107:60,108:70,109:80,110:90,111:70,112:60")]
    public void Test_ApplyOrders(RoomSetuper.Variant variant, string expected) =>
        Assert.That(
            GetAppliedOrderPlacements(variant).ToString(",", p => $"{p.Data.Id}:{p.Position.z}", p => p.Data.Id),
            Is.EqualTo(expected));

    [Test]
    [TestCase(RoomSetuper.Variant._3x2_v1, "1:0,2:1")]
    [TestCase(RoomSetuper.Variant._3x2_v2, "1:0,2:1")]
    [TestCase(RoomSetuper.Variant._5x5_v1, "1:0,2:1,3:2,4:3,5:2,6:3,7:5,8:5,9:7,10:8,11:10,12:9,13:12")]
    [TestCase(RoomSetuper.Variant._5x5_v2, "1:0,2:1,3:2,4:3,5:2,6:12,7:3,8:5,9:5,10:7,11:8,12:9,13:11")]
    [TestCase(RoomSetuper.Variant._33x48_v1, "101:0,102:5,103:2,104:3,105:4,106:5,107:6,108:7,109:8,110:9,111:7,112:6")]
    public void Test_CalculateWeight(RoomSetuper.Variant variant, string expected) =>
        Assert.That(GetCalculatedWeight(variant).ToString(",", p => $"{p.Key}:{p.Value}", true), Is.EqualTo(expected));

    [Test]
    [TestCase(RoomSetuper.Variant._3x2_v1, "1,2")]
    [TestCase(RoomSetuper.Variant._3x2_v2, "1,2")]
    [TestCase(RoomSetuper.Variant._5x5_v1, "1,2,3,4,5,6,7,8,9,10,11,12,13")]
    [TestCase(RoomSetuper.Variant._5x5_v2, "1,2,3,4,5,6,7,8,9,10,11,12,13")]
    [TestCase(RoomSetuper.Variant._33x48_v1, "101,102,103,104,105,106,107,108,111,109,112,110")]
    public void Test_SortBuildingsByPath(RoomSetuper.Variant variant, string expected) =>
        Assert.That(GetSortedPlacements(variant).ToString(",", p => $"{p.Data.Id}"), Is.EqualTo(expected));

    [Test]
    [TestCase(3, 2, "1,2,4\n3,5,6")]
    [TestCase(3, 3, "1,2,4\n3,5,7\n6,8,9")]
    [TestCase(5, 5, "1,2,4,7,11\n3,5,8,12,16\n6,9,13,17,20\n10,14,18,21,23\n15,19,22,24,25")]
    public void Test_MatrixWeight(int x, int y, string expected) =>
        Assert.That(GetPathMatrix(x, y).ToString(",", "\n"), Is.EqualTo(expected));

    private IEnumerable<PlacementView> GetAppliedOrderPlacements(RoomSetuper.Variant variant)
    {
        var roomData = _roomSetuper.Get(variant);
        var roomSize = roomData.size;
        var placements = roomData.placements.ToList();
        _orderCalculator.ApplyOrders(roomSize, placements);
        return placements;
    }

    private Dictionary<long, int> GetCalculatedWeight(RoomSetuper.Variant variant) =>
        new PlacementWeightSetter().CalculateWeight(GetSortedPlacements(variant).ToList());

    private IEnumerable<PlacementView> GetSortedPlacements(RoomSetuper.Variant variant)
    {
        var roomData = _roomSetuper.Get(variant);
        var roomSize = roomData.size;
        var placements = roomData.placements.ToList();
        var pathMatrix = _orderCalculator.GenerateMatrix(roomSize.x, roomSize.y);
        return _orderCalculator.SortBuildingsByPath(placements, pathMatrix);
    }

    private int[,] GetPathMatrix(int x, int y) =>
        _orderCalculator.GenerateMatrix(x, y);
}
