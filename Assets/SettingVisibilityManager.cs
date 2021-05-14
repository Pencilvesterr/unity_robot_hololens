using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SettingVisibilityManager : MonoBehaviour
{
    public GameObject SettingsToggle;
    private GameObject[] SettingButtons;
    private Interactable SettingsToggleStatus;
    

    // Start is called before the first frame update
    void Start()
    {
        SettingButtons = GameObject.FindGameObjectsWithTag("SettingsButton");
        SettingsToggleStatus = SettingsToggle.GetComponent<Interactable>();
        SetAllActiveStatus(false);
    }

    private void SetAllActiveStatus(bool status)
    {
        foreach (GameObject button in SettingButtons)
        {
            button.SetActive(status);
        }
    }

    public void ToggleSwitch()
    {
        if (SettingsToggleStatus.IsToggled)
        {
            SetAllActiveStatus(true);
        }
        else
        {
            SetAllActiveStatus(false);
        }
    }
}
