using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCircuitComponet 
{
    public Position position;
    public Rotation rotation;

    public BaseCircuitComponet(Position position, Rotation rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
public class BaseCircuitComponets 
{
    public List<BaseCircuitComponet> posAndRot;


    public BaseCircuitComponets() {
        posAndRot = new List<BaseCircuitComponet>();

    }

    public void AddPosAndRot(Vector3 position, Quaternion rotation)
    {
        var eulerRot = rotation.eulerAngles;
        posAndRot.Add(new BaseCircuitComponet(
            new Position(position.x, position.y, position.z),
            new Rotation(eulerRot.x, eulerRot.y, eulerRot.z)));
    }
}