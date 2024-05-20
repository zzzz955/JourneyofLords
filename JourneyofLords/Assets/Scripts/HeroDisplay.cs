using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroDisplay : MonoBehaviour
{
    public Image spriteImage;
    public Image lvImage;
    public TMP_Text levelText;

    public void SetHeroData(Hero hero)
    {
        // 스프라이트 설정
        Sprite sprite1 = Resources.Load<Sprite>(hero.spriteName);
        if (sprite1 != null)
        {
            spriteImage.sprite = sprite1;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.spriteName);
        }

        Sprite sprite2 = Resources.Load<Sprite>(hero.att);
        if (sprite2 != null)
        {
            lvImage.sprite = sprite2;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.att);
        }

        // 텍스트 필드 설정
        levelText.SetText(hero.level.ToString());
        
    }
}
