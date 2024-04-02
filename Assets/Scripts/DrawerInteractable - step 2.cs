using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractableStep2 : XRGrabInteractable
{
    [SerializeField] Transform drawerTransform;
    [SerializeField] XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;

    private Transform parentTransform;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";
    private bool isGrabbed = false;
    void Start()
    {
        if(keySocket != null)
        {
            keySocket.selectEntered.AddListener(onDrawerUnlocked);
            keySocket.selectExited.AddListener(onDrawerLocked);
        }
        parentTransform = transform.parent.transform;
        
    }

    private void onDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log("**** DRAWER LOCKED ****");
    }

    private void onDrawerUnlocked(SelectEnterEventArgs arg0)
    {
        isLocked = false;
        Debug.Log("****  DRAWER UNLOCKED *****");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if(!isLocked)
        {
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }
        else
        {
            interactionLayers = InteractionLayerMask.GetMask(defaultLayer);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactionLayers = InteractionLayerMask.GetMask(grabLayer);
        isGrabbed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(isGrabbed && drawerTransform != null)
        {
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x, 
                drawerTransform.localPosition.y, transform.localPosition.z);
        }
    }
}
