[System.Serializable]
public class Tanks : GreenHouseItem
{
    public float diameter;
    public float height;


    public Tanks(string id, float diameter, float height, Position[] pos, Rotation rot) : base(id, pos, rot)
    {
        this.diameter = diameter;
        this.height = height;
    }
}