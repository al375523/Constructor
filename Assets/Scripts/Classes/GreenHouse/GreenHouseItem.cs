using UnityEngine;

public abstract class GreenHouseItem: MonoBehaviour
{
    public string ID;
    public Position[] Position;
    public Rotation Rotation;
   public Vector3 pivotPosition;

    public GreenHouseItem()
    {
    }

    public GreenHouseItem(string id)
    {
        ID = id;
    }

    public GreenHouseItem(string id, Position pivotPosition, Position[] pos, Rotation rot)
    {
        ID = id;
        Position = pos;
        Rotation = rot;
       this.pivotPosition = pivotPosition.ToVector3();
    }
    public GreenHouseItem(string id, Position[] pos, Rotation rot)
    {
        ID = id;
        Position = pos;
        Rotation = rot;
    }
    
}