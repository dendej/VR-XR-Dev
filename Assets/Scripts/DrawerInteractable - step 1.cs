using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractableStep1 : MonoBehaviour
{
    [SerializeField] XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;
    void Start()
    {
        if(keySocket != null)
        {
            keySocket.selectEntered.AddListener(onDrawerUnlocked);
            keySocket.selectExited.AddListener(onDrawerLocked);
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
