using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventCulling : MonoBehaviour
{
    public SkinnedMeshRenderer Renderer;

    private void Start()
    {
        Renderer ??= GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if ((Camera.main == null) || (Renderer == null))
        {
            return;
        }

        Bounds adjustedBounds = Renderer.bounds;
        adjustedBounds.center = Camera.main.transform.position + (Camera.main.transform.forward * (Camera.main.farClipPlane - Camera.main.nearClipPlane) * 0.5f);
        adjustedBounds.extents = new Vector3(0.1f, 0.1f, 0.1f);

        Renderer.bounds = adjustedBounds;
    }
}
