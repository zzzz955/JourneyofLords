using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableHero : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Hero heroData;
    private static GameObject selectedHero;
    private Outline outline;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private GameObject placeholder;
    private int originalIndex;

    private void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.red;
        outline.effectDistance = new Vector2(5, 5);
        outline.enabled = false;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedHero == null)
        {
            // 첫 번째 영웅 선택
            selectedHero = gameObject;
            outline.enabled = true;
        }
        else if (selectedHero == gameObject)
        {
            // 선택 취소
            outline.enabled = false;
            selectedHero = null;
        }
        else
        {
            // 선택된 영웅과 위치 교환
            SwapPositions(selectedHero, gameObject);
            selectedHero.GetComponent<Outline>().enabled = false;
            selectedHero = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (selectedHero == null)
        {
            selectedHero = gameObject;
            outline.enabled = true;
        }

        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();

        placeholder = new GameObject("Placeholder");
        placeholder.transform.SetParent(originalParent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = rectTransform.sizeDelta.x;
        le.preferredHeight = rectTransform.sizeDelta.y;
        placeholder.transform.SetSiblingIndex(originalIndex);

        transform.SetParent(originalParent.parent);

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
        int newIndex = GetNewSiblingIndex();
        placeholder.transform.SetSiblingIndex(newIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Destroy(placeholder);

        outline.enabled = false;
        selectedHero = null;
    }

    private int GetNewSiblingIndex()
    {
        for (int i = 0; i < originalParent.childCount; i++)
        {
            if (transform.position.x < originalParent.GetChild(i).position.x)
            {
                return i;
            }
        }
        return originalParent.childCount;
    }

    private void SwapPositions(GameObject hero1, GameObject hero2)
    {
        Transform parent1 = hero1.transform.parent;
        Transform parent2 = hero2.transform.parent;

        int siblingIndex1 = hero1.transform.GetSiblingIndex();
        int siblingIndex2 = hero2.transform.GetSiblingIndex();

        hero1.transform.SetParent(parent2);
        hero1.transform.SetSiblingIndex(siblingIndex2);

        hero2.transform.SetParent(parent1);
        hero2.transform.SetSiblingIndex(siblingIndex1);
    }
}
