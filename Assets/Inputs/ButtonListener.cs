using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Events;

public class ButtonListener : MonoBehaviour
{
    public UnityEvent proximityEvent;
    public UnityEvent contactEvent;
    public UnityEvent actionEvent;
    public UnityEvent defaultEvent;

    public bool cooldown;
    public bool needsCooldown = true;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = false;
        GetComponent<ButtonController>().InteractableStateChanged.AddListener(InitiateEvent);
    }

    void InitiateEvent(InteractableStateArgs state)
    {
        if (state.NewInteractableState == InteractableState.ProximityState && !cooldown && proximityEvent != null)
        {
            cooldown = true;
            proximityEvent.Invoke();
            StartCoroutine(CoolDown());
        }
        else if (state.NewInteractableState == InteractableState.ContactState && contactEvent != null)
            contactEvent.Invoke();
        else if (state.NewInteractableState == InteractableState.ActionState && actionEvent != null)
            actionEvent.Invoke();
        else if (defaultEvent != null)
            defaultEvent.Invoke();
    }

    public void OnRaycastReceived()
    {
        if (needsCooldown)
        {
            if (!cooldown)
            {
                cooldown = true;
                StartCoroutine(CoolDown());
                proximityEvent.Invoke();               
            }
        }
        else
        {
            proximityEvent.Invoke();
        }
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
}