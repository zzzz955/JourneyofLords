using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes : MonoBehaviour
{
    public GameObject recruitHeroes;
    public GameObject listHeroes;
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
        recruitHeroes.SetActive(true);
    }

    public void RecruitQuit() {
        recruitHeroes.SetActive(false);
    }

    public void Recruit1 () {

    }

    public void Recruit2 () {
        
    }

    public void Recruit3 () {
        
    }

    // 영웅 목록
    public void ListActive() {
        listHeroes.SetActive(true);
    }

    public void ListQuit() {
        listHeroes.SetActive(false);
    }
}
