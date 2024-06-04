using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dragSpeed = 0.1f;
    public float limitXpos = 0f;
    public float limitYpos = 0f;
    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        transform.Translate(move, Space.World);
        
        // Prevent camera from moving outside background bounds
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x, -limitXpos, limitXpos); // X 축 이동 제한
        newPos.y = Mathf.Clamp(newPos.y, -limitYpos, limitYpos); // Y 축 이동 제한
        transform.position = newPos;
    }
}
