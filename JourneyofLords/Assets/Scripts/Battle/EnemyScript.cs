using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    public Hero heroData;
    public TMP_Text troops;
    public Image spriteImage;

    public void Initialize(Hero hero)
    {
        heroData = hero;
        Sprite sprite1 = Resources.Load<Sprite>(heroData.spriteName);
        if (sprite1 != null) {spriteImage.sprite = sprite1;}
        troops.SetText(heroData.lead.ToString());
    }
}
