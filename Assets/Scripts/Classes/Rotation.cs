using UnityEngine;

[System.Serializable]
public class Rotation
{
    public float x, y, z;

    public Rotation(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public Quaternion ToQuaternion()
    {
        return Quaternion.Euler(x, y, z);

    }

    public Vector3 ToEuler()
    {
        return new Vector3(x, y, z);
    }
}