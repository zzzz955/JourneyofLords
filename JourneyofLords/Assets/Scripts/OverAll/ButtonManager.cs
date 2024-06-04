using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // 씬 변환
    public void LoadMainScene() {SceneManager.LoadScene("Main");}
    public void LoadBattleGroundScene() {SceneManager.LoadScene("BattleGround");}
}
