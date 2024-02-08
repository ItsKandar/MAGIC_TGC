// Make the buttons change the camera priorities
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public Button buttonBibliotheque;
    public Button buttonTable;
    public Button buttonQuitter;

    public CinemachineVirtualCamera cameraCloset;
    public CinemachineVirtualCamera cameraTable;
    public CinemachineVirtualCamera cameraOutside;

    void SwitchCamera(CinemachineVirtualCamera cameraToActivate)
    {
        // Set the priority high for the selected camera, and lower for others
        cameraCloset.Priority = (cameraToActivate == cameraCloset) ? 10 : 0;
        cameraTable.Priority = (cameraToActivate == cameraTable) ? 10 : 0;
        cameraOutside.Priority = (cameraToActivate == cameraOutside) ? 10 : 0;
    }

    void Start()
    {
        buttonBibliotheque.onClick.AddListener(() => SwitchCamera(cameraCloset));
        buttonTable.onClick.AddListener(() => SwitchCamera(cameraTable));
        buttonQuitter.onClick.AddListener(() => SwitchCamera(cameraOutside));
    }
}