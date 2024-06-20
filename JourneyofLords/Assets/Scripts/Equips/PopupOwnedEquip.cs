using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class popupOwnEquipPrefab : MonoBehaviour
{
    public Button [] buttons;
    public Color selectedColor = Color.green;
    private Button selectedButton;
    private Color originalColor;
    private GameManager gameManager;

    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText;
    public GameObject popupEquipPrefab;

    public GameObject levelUp;
    public GameObject gradeUp;

    private float tempAtk;
    private float tempDef;
    private float tempHp;

    public Equip currentEquip { get; private set; }
    private GameObject currentPrefab;

    public void SetEquitData(Equip equip)
    {
        currentEquip = equip;
        tempAtk = equip.atk;
        tempDef = equip.def;
        tempHp = equip.hp;

        // 스프라이트 설정
        if (spriteImage != null) {
            Sprite sprite1 = Resources.Load<Sprite>(equip.spriteName);
            if (sprite1 != null) {spriteImage.sprite = sprite1;}
        }

        if (levelText != null) {levelText.SetText(equip.level.ToString());}
    }

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
        // ShowPrefab(button.name);
    }

    // void ShowPrefab (string name) {
    //     GameObject currentPrefab = Instantiate(equipSlot, equipTransfrom);
    //     EquipDisplay equipDisplay = slot.GetComponent<EquipDisplay>();
    //     equipDisplay.SetEquitData(equip);
    // }
}
