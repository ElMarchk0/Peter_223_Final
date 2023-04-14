using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    // Delegates to select prefab to to build
    public Action OnRoadPlacement;
    public Action OnHousePlacement;
    public Action OnSpecialPlacement;
    public Action OnBigStructurePlacement;

    public Button placeHouseButton;
    public Button placeRoadButton;
    public Button placeSpecialButton;
    public Button placeBigButton;

    public Color outlineColor;
    List<Button> buttonList;

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, placeBigButton };

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

        placeBigButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeBigButton);
            OnBigStructurePlacement?.Invoke();

        });
    }

    // Change the outline color of a button to indicate which build component was selected
    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    // Remove the outline color of a button
    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
