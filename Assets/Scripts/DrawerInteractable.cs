using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : XRGrabInteractable
{
    [SerializeField] Transform drawerTransform;
    [SerializeField] XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;

    private Transform parentTransform;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";
    private bool isGrabbed = false;
    private Vector3 limitPositions;
    [SerializeField] private Vector3 limitDistances = new Vector3 (0.02f, 0.02f, 0);
    void Start()
    {
        if(keySocket != null)
        {
            keySocket.selectEntered.AddListener(onDrawerUnlocked);
            keySocket.selectExited.AddListener(onDrawerLocked);
        }
        parentTransform = transform.parent.transform;
        limitPositions = drawerTransform.localPosition;
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
            ChangeLayerMask(defaultLayer); 
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        isGrabbed = false;
        transform.localPosition = drawerTransform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if(drawerTransform != null)
        {
            if (isGrabbed)
            {
                drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x,
                    drawerTransform.localPosition.y, transform.localPosition.z);
                CheckLimits();
            }
        }
        
    }

    private void CheckLimits()
    {
        if(transform.localPosition.x >= limitPositions.x + limitDistances.x ||
            transform.localPosition.x <= limitPositions.x - limitDistances.x ||
            transform.localPosition.y >= limitPositions.y + limitDistances.y ||
            transform.localPosition.y <= limitPositions.y - limitDistances.y)
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
