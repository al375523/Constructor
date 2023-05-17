using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Toggle toggle;
    public void ChangeToggle()
    {
        toggle.isOn = !toggle.isOn;
    }
}
