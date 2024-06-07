using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color hoverColor = Color.white;
    public Color clickColor = Color.gray;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isClicked = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalColor = GetComponent<Image>().color; // Assuming the Button has an Image component
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
        {
            transform.localScale = originalScale * 1.1f;
            GetComponent<Image>().color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
        {
            transform.localScale = originalScale;
            GetComponent<Image>().color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;
        transform.localScale = originalScale * 1.1f;
        GetComponent<Image>().color = clickColor;
    }
}
