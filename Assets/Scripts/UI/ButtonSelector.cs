using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    private PauseMenu menu;

    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();

        menu = transform.parent.parent.GetComponent<PauseMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.ClearSelectors();
        menu.SelectedButton = rect;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menu.ClearSelectors();
        menu.SelectedButton = null;
    }
}
