using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofCanvasControl : MonoBehaviour
{
    Animator animator;
    bool disabled;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("LonaControl");
        disabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.98f && !disabled)
        {
            animator.enabled = false;
            disabled = true;
        }
    }
}
