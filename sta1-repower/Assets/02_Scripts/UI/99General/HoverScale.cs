using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleMultiplier = 1.1f; 
    private Vector3 originalScale; 

    void Start()
    {
        originalScale = transform.localScale; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleMultiplier; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; 
    }
}
