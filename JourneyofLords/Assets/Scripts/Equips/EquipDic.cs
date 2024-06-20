using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EquipDic : MonoBehaviour
{
    public Button [] buttons;
    public Color selectedColor = Color.green;
    public GameObject equipSlot;
    public Transform equipTransfrom;

    private Button selectedButton;
    private Color originalColor;
    
    private GameManager gameManager;
    private List<Equip> allEquips;

    void Start()
    {
        gameManager = GameManager.Instance;
        allEquips = gameManager.equipList;
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
        ClearLayout();
        ShowEquips(button.name);
    }

    void ClearLayout () {
        foreach (Transform child in equipTransfrom)
        {
            Destroy(child.gameObject);
        }
    }

    void ShowEquips (string name) {
        List<Equip> equips = allEquips.Where(equip => equip.type == name).ToList();
        foreach (Equip equip in equips) {
            GameObject slot = Instantiate(equipSlot, equipTransfrom);
            EquipDisplay equipDisplay = slot.GetComponent<EquipDisplay>();
            equipDisplay.SetEquitData(equip);
        }
    }
}
