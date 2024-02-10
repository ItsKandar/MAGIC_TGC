// Make the buttons change the camera priorities
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public Button buttonBibliotheque;
    public Button buttonTable;
    public Button buttonQuitter;

    public Button BackFromCloset;
    public Button BackFromTable;

    public Button BackFromOutside;
    public Button ConfirmQuit;
    
    public GameObject TVUI;
    public GameObject ClosetUI;
    public GameObject TableUI;
    public GameObject ExitUI;

    public CinemachineVirtualCamera cameraTV;
    public CinemachineVirtualCamera cameraCloset;
    public CinemachineVirtualCamera cameraTable;
    public CinemachineVirtualCamera cameraOutside;

    void SwitchCamera(CinemachineVirtualCamera cameraToActivate)
    {
        Debug.Log("Switching to camera: " + cameraToActivate);

        // Camera Closet
        cameraCloset.Priority = (cameraToActivate == cameraCloset) ? 10 : 0;
        if (cameraToActivate == cameraCloset)
        {   
            DisableUIs(TVUI);
            EnableUIs(ClosetUI);
            BackFromCloset.onClick.AddListener(() => SwitchCamera(cameraTV));
        }

        // Camera Table
        cameraTable.Priority = (cameraToActivate == cameraTable) ? 10 : 0;
        if (cameraToActivate == cameraTable)
        {
            DisableUIs(TVUI);
            EnableUIs(TableUI);
            BackFromTable.onClick.AddListener(() => SwitchCamera(cameraTV));
        }

        // Camera Outside
        cameraOutside.Priority = (cameraToActivate == cameraOutside) ? 10 : 0;
        if (cameraToActivate == cameraOutside)
        {
            DisableUIs(TVUI);
            EnableUIs(ExitUI);
            ConfirmQuit.onClick.AddListener(() => Application.Quit());
            BackFromOutside.onClick.AddListener(() => SwitchCamera(cameraTV));
        }

        // Camera TV
        cameraTV.Priority = (cameraToActivate == cameraTV) ? 10 : 0;
        if (cameraToActivate == cameraTV)
        {
            DisableUIs(TableUI);
            DisableUIs(ClosetUI);
            DisableUIs(ExitUI);
            EnableUIs(TVUI);
        }
    }

    void DisableUIs(GameObject UIToDisable)
    {
        UIToDisable.SetActive(false);
    }

    void EnableUIs(GameObject UItoEnable)
    {
        UItoEnable.SetActive(true);
    }

    void Start()
    {
        // Add listeners to the buttons
        buttonBibliotheque.onClick.AddListener(() => SwitchCamera(cameraCloset));
        buttonTable.onClick.AddListener(() => SwitchCamera(cameraTable));
        buttonQuitter.onClick.AddListener(() => SwitchCamera(cameraOutside));

        
        // Disable UIs
        ClosetUI.SetActive(false);
        TableUI.SetActive(false);
        ExitUI.SetActive(false);

        // Enable TV UI
        TVUI.SetActive(true);

        // Start with the TV camera
        SwitchCamera(cameraTV);
    }
}