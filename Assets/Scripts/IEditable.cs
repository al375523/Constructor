using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditableLength
{
    void EditLength(float delta);
    bool CanEditLength();
}
public interface IEditableWaterCirculation
{
    void EditWaterCirculation();
    bool CanEditWaterCirculation();
}
public interface IEditableHorizontalRotation
{
    void EditHorizontalRotation(bool shouldAdjustPosition);
    bool CanEditHorizontalRotation();
}
public interface IEditableVerticalRotation
{
    void EditVerticalRotation(bool shouldAdjustPosition);
    bool CanEditVerticalRotation();

}
public interface IEditableChangeSize
{
    void EditChangeSize();
    bool CanEditChangeSize();
}

public interface IMoveable
{
    void Move(Vector3 position);
    bool CanMove(Vector3 position);

}