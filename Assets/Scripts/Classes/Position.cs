using System;
using UnityEngine;

[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;

    public Position(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Position()
    {
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public static Position operator - (Position pos1, Position pos2)
    {
        return new Position(pos2.x - pos1.x, pos2.y - pos1.y, pos2.z - pos1.z);
    }

    public static Position operator + (Position pos1, Position pos2)
    {
        return new Position(pos2.x + pos1.x, pos2.y + pos1.y, pos2.z + pos1.z);
    }

    public static Position operator / (Position pos2, Position pos1)
    {
        return new Position((pos2.x + pos1.x) / 2.0f, (pos2.y + pos1.y) / 2.0f, (pos2.z + pos1.z) / 2.0f);
    }

    public Position FromVector3(Vector3 position)
    {
        return new Position(position.x, position.y, position.z);
    }
}