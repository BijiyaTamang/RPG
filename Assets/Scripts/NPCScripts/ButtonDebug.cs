using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDebug : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("BUTTON CLICKED: " + gameObject.name);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("BUTTON HOVERED: " + gameObject.name);
    }
}