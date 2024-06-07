using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonGroupManager : MonoBehaviour
{
    public List<Button> buttons;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.1f;

    private Dictionary<Button, Vector3> originalScales;
    private Dictionary<Button, Color> originalColors;
    private Dictionary<Button, TMP_Text> buttonTexts;
    private Button selectedButton;

    void Start()
    {
        originalScales = new Dictionary<Button, Vector3>();
        originalColors = new Dictionary<Button, Color>();
        buttonTexts = new Dictionary<Button, TMP_Text>();

        foreach (var button in buttons)
        {
            originalScales[button] = button.transform.localScale;
            var text = button.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                buttonTexts[button] = text;
                originalColors[button] = text.color;
            }

            AddEventTriggers(button);
        }
    }

    private void AddEventTriggers(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => OnPointerEnter(button));
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => OnPointerExit(button));
        trigger.triggers.Add(entryExit);

        EventTrigger.Entry entryClick = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entryClick.callback.AddListener((data) => OnPointerClick(button));
        trigger.triggers.Add(entryClick);
    }

    public void OnPointerEnter(Button button)
    {
        if (button != selectedButton)
        {
            button.transform.localScale = originalScales[button] * hoverScale;
            if (buttonTexts.ContainsKey(button))
            {
                buttonTexts[button].color = hoverColor;
            }
            if (hoverAudioSource != null)
            {
                hoverAudioSource.Play();
            }
        }
    }

    public void OnPointerExit(Button button)
    {
        if (button != selectedButton)
        {
            button.transform.localScale = originalScales[button];
            if (buttonTexts.ContainsKey(button))
            {
                buttonTexts[button].color = originalColors[button];
            }
        }
    }

    public void OnPointerClick(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.transform.localScale = originalScales[selectedButton];
            if (buttonTexts.ContainsKey(selectedButton))
            {
                buttonTexts[selectedButton].color = originalColors[selectedButton];
            }
        }

        selectedButton = button;
        button.transform.localScale = originalScales[button] * hoverScale;
        if (buttonTexts.ContainsKey(button))
        {
            buttonTexts[button].color = selectedColor;
        }
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
    }
}
