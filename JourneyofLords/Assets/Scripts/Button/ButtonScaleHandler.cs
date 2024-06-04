using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;

    void Start()
    {
        // 원래 스케일 저장
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 마우스 좌클릭 다운 시 스케일 변경
        transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 마우스 좌클릭 업 시 원래 스케일로 복귀
        transform.localScale = originalScale;
    }
}
