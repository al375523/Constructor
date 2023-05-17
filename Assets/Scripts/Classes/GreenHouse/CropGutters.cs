[System.Serializable]
public class CropGutters : GreenHouseItem
{
    public CropGutters(string ID, Position[] pos, Rotation rotation)
    {
        this.ID = ID;
        Position = pos;
        Rotation = rotation;
    }


}