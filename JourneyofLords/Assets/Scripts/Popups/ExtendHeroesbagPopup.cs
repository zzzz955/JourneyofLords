using UnityEngine;
using TMPro;

public class ExtendHeroesbagPopup : MonoBehaviour
{
    public TMP_Text extendCnt;
    public TMP_Text needGoldCnt;
    public GameObject extendHeroesbag;
    private int cnt;
    private HeroManager heroManager;

    public void Initialize(HeroManager manager)
    {
        heroManager = manager;
    }

    public void CheckVal(int num) {
        ExtendHeroesBagActive();
        cnt = num;
        extendCnt.SetText("금화를 사용하여 " + num + "개 슬롯을 확장 하시겠습니까?");
        needGoldCnt.SetText((num * 5).ToString());
    }

    public void ExtentionHeroesbag () {
        if (heroManager != null)
        {
            heroManager.Maxpuls(cnt);
        }
        ExtendHeroesBagQuit();
    }

    public void ExtendHeroesBagActive () {
        extendHeroesbag.SetActive(true);
    }

    public void ExtendHeroesBagQuit () {
        extendHeroesbag.SetActive(false);
    }
}
