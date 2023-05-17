using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;
public class WaterPassShader : MonoBehaviour
{
    public bool waterIsOpen = false;
    internal void ShowClose(List<GameObject> circuitElements)
    {
        foreach (var item in circuitElements)
        {
            var meshes=item.GetComponentsInChildren<Renderer>();
            foreach (var mesh in meshes)
            {

                foreach (var mat in mesh.materials)
                {
                    mat.SetFloat("_WaterPassOn", 0.0f);
                }
            }
        }
    }

    internal void ShowOpen(List<GameObject> waterPassGOs)
    {
        foreach (var item in waterPassGOs)
        {
            var meshes = item.GetComponentsInChildren<Renderer>();
            foreach (var mesh in meshes)
            {

                foreach (var mat in mesh.materials)
                {
                    mat.SetFloat("_WaterPassOn", 1.0f);
                }
            }
        }
    }
    internal void ChangWaterPass() {
        waterIsOpen = !waterIsOpen;
    }
}
