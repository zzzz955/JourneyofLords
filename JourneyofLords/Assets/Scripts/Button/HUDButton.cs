using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDButton : MonoBehaviour
{
    public GameObject popupPrefab;
    private PopupManager popupManager;

    private void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    public void OnButtonClick()
    {
        popupManager.ShowPopup(popupPrefab);
    }
}