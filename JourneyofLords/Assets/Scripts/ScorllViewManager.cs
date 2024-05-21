using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public RectTransform content; // Content의 RectTransform
    public float itemHeight; // 각 아이템의 높이
    public float spacing; // 아이템 간의 간격
    public int itemCount; // 아이템의 개수

    void Start()
    {
        AdjustContentHeight();
    }

    void AdjustContentHeight()
    {
        if (content != null)
        {
            float totalHeight = itemCount * (itemHeight + spacing) - spacing; // 전체 높이 계산
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight); // Content의 높이 설정
        }
    }

    // 아이템을 추가하거나 제거할 때 호출하여 Content 높이 조정
    public void UpdateContentHeight(int newItemCount)
    {
        itemCount = newItemCount;
        AdjustContentHeight();
    }
}
