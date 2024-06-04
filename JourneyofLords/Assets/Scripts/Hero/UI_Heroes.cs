using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes : MonoBehaviour
{
    public GameObject recruitHeroesUI;
    public GameObject dicHeroesUI;
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }

    private void ClosePopup()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    // 영웅 모집
    public void RecruitActive() {
        recruitHeroesUI.SetActive(true);
    }

    public void RecruitQuit() {
        recruitHeroesUI.SetActive(false);
    }


    public void Recruit2 () {
        
    }

    public void Recruit3 () {
        
    }

    // 영웅 목록
    public void ListActive() {
        dicHeroesUI.SetActive(true);
    }

    public void ListQuit() {
        dicHeroesUI.SetActive(false);
    }
}
