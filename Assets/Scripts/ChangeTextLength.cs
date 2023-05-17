using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTextLength : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;

    public void ChangeText(string text)
    {
        infoText.text = text;
    }

}
