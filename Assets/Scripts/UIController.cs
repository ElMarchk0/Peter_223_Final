using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement; 
    public Action OnHousePlacement; 
    public Action OnSpecialPlacement; 
    public Action OnBigStructurePlacement; 
    public Action OnPoliceStationPlacement;
    public Action OnHospitalPlacement; 
    public Action OnFireStationPlacement;
    public Button placeRoadButton; 
    public Button placeHouseButton; 
    public Button placeSpecialButton; 
    public Button placeBigStructureButton; 
    public Button placePoliceStationButton;
    public Button placeHospitalButton; 
    public Button placeFireStationButton;

    public Color outlineColor;
    List<Button> buttonList;

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, placeBigStructureButton, placePoliceStationButton, placeHospitalButton, placeFireStationButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();

        });
        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();

        });
        placeSpecialButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeSpecialButton);
            OnSpecialPlacement?.Invoke();

        });
        placeBigStructureButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeBigStructureButton);
            OnBigStructurePlacement?.Invoke();
        });
        placePoliceStationButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placePoliceStationButton);
            OnPoliceStationPlacement?.Invoke();
        });
        placeHospitalButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHospitalButton);
            OnHospitalPlacement?.Invoke();
        });
        placeFireStationButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeFireStationButton);
            OnFireStationPlacement?.Invoke();
        });
    }

    // Apply the button outline
    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    // Remove the outline from around the button
    public void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
