using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipDic : MonoBehaviour
{
    public Button [] buttons;
    public Color selectedColor = Color.green;

    private Button selectedButton;
    private Color originalColor;
    
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        if (buttons.Length > 0)
        {
            selectedButton = buttons[0];
            originalColor = selectedButton.image.color;
            SetSelectedButton(selectedButton);
        }
    }

    void OnButtonClick(Button button)
    {
        if (button != selectedButton)
        {
            SetSelectedButton(button);
        }
    }

    void SetSelectedButton(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.image.color = originalColor;
        }
        selectedButton = button;
        selectedButton.image.color = selectedColor;
        Debug.Log(button.name);
    }

    void ShowEquips (string name) {

    }
}
