namespace UniTests;

public class PlacementView
{
    public PlaceArea Area;
    public Vector3Int Position => Area.Position;
    public PlacementData Data;
}

public class PlacementData
{
    public long Id;
}
