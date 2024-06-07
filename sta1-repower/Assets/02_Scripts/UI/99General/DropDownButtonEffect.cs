using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DropdownButtonEffects : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.05f;
    private Color originalColor;
    private Vector3 originalScale;

    void Start()
    {
        originalColor = dropdown.GetComponent<Image>().color;
        originalScale = dropdown.transform.localScale;

        AddEventTriggers(dropdown);
    }

    private void AddEventTriggers(TMP_Dropdown dropdown)
    {
        EventTrigger trigger = dropdown.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((eventData) => OnHoverEnter());
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((eventData) => OnHoverExit());
        trigger.triggers.Add(entryExit);

        dropdown.onValueChanged.AddListener((value) => OnSubmit());
    }

    public void OnHoverEnter()
    {
        dropdown.transform.localScale = originalScale * hoverScale;
        dropdown.GetComponent<Image>().color = hoverColor;
        if (hoverAudioSource != null)
        {
            hoverAudioSource.Play();
        }
    }

    public void OnHoverExit()
    {
        dropdown.transform.localScale = originalScale;
        dropdown.GetComponent<Image>().color = originalColor;
    }

    public void OnSubmit()
    {
        dropdown.GetComponent<Image>().color = selectedColor;
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
    }
}
