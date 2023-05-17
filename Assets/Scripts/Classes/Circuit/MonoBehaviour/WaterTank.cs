using System;
using UnityEngine;

/// <summary>
/// /// pump
/// </summary>
[System.Serializable]
public class WaterTank : CircuitItem, IEditableHorizontalRotation, IEditableVerticalRotation, IMoveable
{
    public WaterTank(string id, Transform initNormal, Transform endNormal, float prevWidth, string modelo) : base(id, initNormal,
        endNormal, prevWidth, modelo)
    {
    }


    public override void ScaleItemDiameter(float currentSize, float targetSize)
    {
        //todo change waterTank
    }

    public override void SelectItemJson()
    {
        return;
    }


    #region IEditable
    bool IEditableHorizontalRotation.CanEditHorizontalRotation()
    {
        return nextItem == null;
    }

    bool IEditableVerticalRotation.CanEditVerticalRotation()
    {
        return prevItem == null && nextItem==null;
    }

    void IEditableHorizontalRotation.EditHorizontalRotation(bool shouldAdjustPosition)
    {
        gameObject.transform.Rotate(90, 0, 0);
        if (prevItem != null && shouldAdjustPosition)
        {
            Vector3 DNI = transform.position - initialNormal.position;
            gameObject.transform.position = prevItem.endNormal.position + DNI;
        }
    }

    void IEditableVerticalRotation.EditVerticalRotation(bool shouldAdjustPosition)
    {
        gameObject.transform.Rotate(0, 90, 0);
        if (prevItem != null && shouldAdjustPosition) {
            Vector3 DNI = transform.position - initialNormal.position;
            gameObject.transform.position = prevItem.endNormal.position + DNI;
        }

    }

    bool IMoveable.CanMove(Vector3 position)
    {
        return true;
    }

    void IMoveable.Move(Vector3 position)
    {
        this.transform.position += position;
    }


    #endregion
}