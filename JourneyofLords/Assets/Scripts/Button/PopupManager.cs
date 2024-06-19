using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public Canvas popupCanvas;
    private GameObject currentPopup;

    private void Update()
    {
        // ESC 키를 눌렀을 때 팝업을 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HidePopup();
        }
    }

    public void ShowPopup(GameObject popupPrefab)
    {
        currentPopup = Instantiate(popupPrefab, popupCanvas.transform);
        currentPopup.SetActive(true);
    }

    public void HidePopup()
    {
        if (currentPopup != null)
        {
            currentPopup.SetActive(false);
            Destroy(currentPopup);
        }
    }
}