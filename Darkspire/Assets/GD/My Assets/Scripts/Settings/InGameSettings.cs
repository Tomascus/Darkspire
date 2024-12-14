using InputSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Reference https://www.youtube.com/watch?v=HnvPNoU9Wjw

public class InGameSettings : MonoBehaviour
{
    #region ResolutionFields

    private GameObject settingMenu;
    private PlayerControllerInputs playerControllerInputs;

    [SerializeField] private TMP_Dropdown resolutionDropdown;   //Allows to use the current resolution dropdown
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;   //Contains resolution with the monitor

    private float currentRefreshRate;   //Current refresh rate of the monitor
    private int currentResolutionIndex = 0; //Current resolution index

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //Turn off InGameSettings once escape is pressed or unpause
        {
            settingMenu.SetActive(false);   //Turn off the object
            playerControllerInputs.HideCursor(); //Hide the cursor
        }
    }



    private void Start()
    {
        resolutions = Screen.resolutions;               
        filteredResolutions = new List<Resolution>();   

        resolutionDropdown.ClearOptions();  //Clears the dropdown options
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;    //Current refresh rate 

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Mathf.Approximately(resolutions[i].refreshRate, currentRefreshRate))            //If the refresh rate is equal to the current refresh rate
            {
                filteredResolutions.Add(resolutions[i]);    //Add current resolution to the list
            }
        }

        List<string> options = new List<string>();              //List of resolution options
        for (int i = 0; i < filteredResolutions.Count; i++)     //Go throught current filtered resolutions
        {
            string resolutionOptions = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";    //Contains the width,height and Hz of the resoulution
            options.Add(resolutionOptions);     //Add resoulution options to the list
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)     //Check which one matches it
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);                 //Add the options to the dropdown
        resolutionDropdown.value = currentResolutionIndex;      //Set the value of the dropdown to current resolution index
        resolutionDropdown.RefreshShownValue();                 //Show the value of the dropdown
    }

    public void ChangeResolution(int resolution)
    {
        if (resolution >= 0 && resolution < filteredResolutions.Count)
        {
            Resolution resolutionSet = filteredResolutions[resolution];
            Screen.SetResolution(resolutionSet.width, resolutionSet.height, Screen.fullScreenMode);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}