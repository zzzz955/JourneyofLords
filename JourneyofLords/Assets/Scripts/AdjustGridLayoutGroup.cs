using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustGridLayoutGroup : MonoBehaviour
{
    public float offsetPerRow = 200.0f; // 한 행 당 이동할 y축 오프셋
    public ScrollRect scrollRect;

    // displayedRows는 표시할 행 수를 의미합니다.
    public void ModifyGrid(int displayedRows)
    {
        if (scrollRect != null)
        {
            RectTransform contentRect = scrollRect.content;

            if (contentRect != null)
            {
                // y 좌표를 행 수에 따라 조정합니다.
                float totalOffset = offsetPerRow * (displayedRows - 3);
                contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y - totalOffset);

                // Content의 크기를 조정하여 스크롤 가능 영역을 확장합니다.
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + totalOffset);
            }
        }
    }
}
