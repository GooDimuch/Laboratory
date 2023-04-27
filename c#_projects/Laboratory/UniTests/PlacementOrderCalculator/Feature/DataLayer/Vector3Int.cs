namespace UniTests;

public struct Vector3Int
{
    public int x;
    public int y;
    public int z;
    public static Vector3Int up = new Vector3Int(0, 1);
    public static Vector3Int right = new Vector3Int(1, 0);
    public static Vector3Int one = new Vector3Int(1, 1, 1);

    public Vector3Int(int x, int y, int z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
}
