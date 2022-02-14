using Assets.Game.Scripts.Utils;
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

    public static string DisplayName { get; set; }
    public static Color DisplayColor { get; set; }

    private const string PlayerPrefsNameKey = "PlayerName";
    

    private void Start()
    {
        SetInputField();
    }

    private void SetUserColor()
    {
        if (!PlayerPrefsExtensions.HasWritenColor)
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
        var color = PlayerPrefsExtensions.GetColor();
        DisplayColor = color;
    }

    private void SetInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
            return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        TextUpdated();
    }

    public void TextUpdated()
    {
        continueButton.interactable = !string.IsNullOrEmpty(nameInputField.text);
    }

    public void SavePlayerName()
    {
        var name = nameInputField.text;
        SetDisplayName(name);

        SetUserColor();
        SetDisplayInfo();
    }

    public static void SetDisplayName(string name)
    {
        DisplayName = name;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

    private void SetDisplayInfo()
    {
        menuUserInfo.SetDisplayUserColor(DisplayColor);
        menuUserInfo.SetDisplayUsername(DisplayName);
        menuUserInfo.gameObject.SetActive(true);
    }
}
