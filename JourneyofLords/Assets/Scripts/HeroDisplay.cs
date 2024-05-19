using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroDisplay : MonoBehaviour
{
    public Image spriteImage;
    public TMP_Text levelText;

    public void SetHeroData(Hero hero)
    {
        // 스프라이트 설정
        Sprite sprite = Resources.Load<Sprite>(hero.spriteName);
        if (sprite != null)
        {
            spriteImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.spriteName);
        }

        // 텍스트 필드 설정
        levelText.SetText(hero.level.ToString());
    }
}
