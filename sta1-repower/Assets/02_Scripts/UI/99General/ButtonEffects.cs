using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.1f;

    private Color originalTextColor;
    private Vector3 originalScale;
    private bool isSelected = false;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }

        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }

        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            transform.localScale = originalScale * hoverScale;
            if (buttonText != null)
            {
                buttonText.color = hoverColor;
            }
            if (hoverAudioSource != null)
            {
                hoverAudioSource.Play();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            transform.localScale = originalScale;
            if (buttonText != null)
            {
                buttonText.color = originalTextColor;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = true;
        if (buttonText != null)
        {
            buttonText.color = selectedColor;
        }
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
    }
}
