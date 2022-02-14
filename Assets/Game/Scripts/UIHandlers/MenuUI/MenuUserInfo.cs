using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUserInfo : MonoBehaviour
{
    [SerializeField] RawImage UserColorImage;
    [SerializeField] TMP_Text UserNameText;
    [SerializeField] UserInfoSettings playerSettings;

    public void SetDisplayUsername(string username)
    {
        UserNameText.text = username;
    }

    public void SetDisplayUserColor(Color color)
    {
        UserColorImage.color = color;
    }

    public void SetRandomDisplayeUserColor()
    {
        var color = Random.ColorHSV();
        color.a = 1;

        UserColorImage.color = color;
    }

    public void OpenSettings()
    {
        playerSettings.OpenSettings();
    }
}
