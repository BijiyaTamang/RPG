using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastDebug : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            foreach (var result in results)
                Debug.Log("HIT: " + result.gameObject.name + " | Depth: " + result.depth + " | SortOrder: " + result.sortingOrder);
        }
    }
}