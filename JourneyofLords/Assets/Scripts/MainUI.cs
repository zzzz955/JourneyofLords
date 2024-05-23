using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    public TMP_Text pLevel;
    public TMP_Text pName;
    public TMP_Text pWood;
    public TMP_Text pStone;
    public TMP_Text pIron;
    public TMP_Text pFood;
    public TMP_Text pGold;
    public TMP_Text pMoney;

    public void UpdatePlayerStatus(User userData) {
        pLevel.SetText(userData.userLV.ToString());
        pName.SetText(userData.IGN);
        pWood.SetText(userData.wood.ToString());
        pStone.SetText(userData.stone.ToString());
        pIron.SetText(userData.iron.ToString());
        pFood.SetText(userData.food.ToString());
        pGold.SetText(userData.gold.ToString());
        pMoney.SetText(userData.money.ToString());
    }
}
