using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    [SerializeField] private MenuUserInfo menuUserInfo = null;

    public static string DisplayName { get; private set; }
    public static Color DisplayColor { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";
    private readonly string[] PlayerPrefsColorKeys = new string[3]
    {
        "PlayerColorR",
        "PlayerColorG",
        "PlayerColorB",
    };

    private void Start()
    {
        SetInputField();
    }

    private void SetUserColor()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsColorKeys[0]))
            SetRandomColor();
        else
            SetColorFromPlayerPrefs();
    }

    private void SetRandomColor()
    {
        var color = Random.ColorHSV();
        color.a = 1;

        DisplayColor = color;
    }

    private void SetColorFromPlayerPrefs()
    {
        var colorR = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[0]);
        var colorG = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[1]);
        var colorB = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[2]);

        var color = new Color(colorR, colorG, colorB);
        DisplayColor = color;
    }

    private void SetInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
            return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName();
    }

    public void SetPlayerName()
    {
        continueButton.interactable = !string.IsNullOrEmpty(nameInputField.text);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);

        SetUserColor();
        SetDisplayInfo();
    }

    private void SetDisplayInfo()
    {
        menuUserInfo.SetDisplayUserColor(DisplayColor);
        menuUserInfo.SetDisplayUsername(DisplayName);
        menuUserInfo.gameObject.SetActive(true);
    }
}
