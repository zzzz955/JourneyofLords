using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        ClickableHero draggableHero = eventData.pointerDrag.GetComponent<ClickableHero>();
        if (draggableHero != null)
        {
            Transform originalParent = draggableHero.transform.parent;

            if (transform.childCount == 0)
            {
                draggableHero.transform.SetParent(transform);
                draggableHero.transform.localPosition = Vector3.zero;
            }
            else
            {
                Transform existingHero = transform.GetChild(0);
                existingHero.SetParent(originalParent);
                existingHero.localPosition = Vector3.zero;

                draggableHero.transform.SetParent(transform);
                draggableHero.transform.localPosition = Vector3.zero;
            }
        }
    }
}
