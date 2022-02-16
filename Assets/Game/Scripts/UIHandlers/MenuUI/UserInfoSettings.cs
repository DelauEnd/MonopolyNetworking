using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.Game.Scripts.Utils;

public class UserInfoSettings : MonoBehaviour
{
    [SerializeField] Slider[] colorSliders = new Slider[3];
    [SerializeField] TMP_InputField usernameField = null;
    [SerializeField] RawImage buildedColorImage = null;
    [SerializeField] MenuUserInfo userInfo = null;

    public void OpenSettings()
    {
        this.gameObject.SetActive(true);

        usernameField.text = PlayerNameInput.DisplayName;
        SetSliders();
    }

    private void SetSliders()
    {
        var userColor = PlayerNameInput.DisplayColor;

        colorSliders[0].value = userColor.r;
        colorSliders[1].value = userColor.g;
        colorSliders[2].value = userColor.b;
    }

    public void OnSliderChange()
    {
        float colorR = colorSliders[0].value;
        float colorG = colorSliders[1].value;
        float colorB = colorSliders[2].value;

        var color = new Color(colorR, colorG, colorB);
        buildedColorImage.color = color;
    }

    public void SaveSettings()
    {
        if (!string.IsNullOrEmpty(usernameField.text))
            PlayerNameInput.SetDisplayName(usernameField.text);

        PlayerNameInput.DisplayColor = buildedColorImage.color;
        PlayerPrefsExtensions.WriteColor(PlayerNameInput.DisplayColor);

        userInfo.SetDisplayUserColor(PlayerNameInput.DisplayColor);
        userInfo.SetDisplayUsername(PlayerNameInput.DisplayName);

        CloseSettings();
    }

    public void CloseSettings()
    {
        this.gameObject.SetActive(false);
    }
}
