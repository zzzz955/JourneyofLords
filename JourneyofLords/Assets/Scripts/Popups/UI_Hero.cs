using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UI_Hero : MonoBehaviour
{
    public GameObject heroImage;
    public Transform createPos;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void Quit() {
        Destroy(gameObject);
    }
}
