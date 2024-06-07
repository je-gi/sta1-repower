using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class SceneFlowManager : MonoBehaviour
{
    public TMP_Text text01;
    public TMP_Text text02;
    public TMP_Text text03;
    public Image image01;
    public Image image02;
    public Button continueButton;
    public Image fadeInImage;
    public float fadeDuration = 1f;
    public float text02DelayAfterText01 = 2f;
    public float text03DelayAfterImage02 = 3f;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.1f;

    private string text01Content;
    private bool isFirstClick = true;
    private Color originalTextColor;
    private Vector3 originalScale;
    private bool isSelected = false;

    void Start()
    {
        text01Content = text01.text;
        text01.text = "";
        text02.gameObject.SetActive(false);
        text03.gameObject.SetActive(false);
        image01.gameObject.SetActive(false);
        image02.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        fadeInImage.gameObject.SetActive(false);

        originalTextColor = continueButton.GetComponentInChildren<TMP_Text>().color;
        originalScale = continueButton.transform.localScale;

        continueButton.onClick.AddListener(OnContinueButtonClick);
        AddEventTriggers(continueButton);

        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        yield return StartCoroutine(FadeOutImage(fadeInImage));
        fadeInImage.gameObject.SetActive(false);

        yield return StartCoroutine(FadeInTextSlowly(text01));
        yield return new WaitForSeconds(2f);

        continueButton.gameObject.SetActive(true);
    }

    IEnumerator FadeInTextSlowly(TMP_Text textComponent)
    {
        float fadeInDuration = fadeDuration * 2;
        float elapsedTime = 0;
        Color originalColor = textComponent.color;
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        textComponent.gameObject.SetActive(true);

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator FadeOutAndLoadNextSequence()
    {
        yield return StartCoroutine(FadeOutText(text01));
        continueButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(text02DelayAfterText01);

        text02.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInText(text02));

        yield return StartCoroutine(FadeInImage(image01));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeInImage(image02));

        yield return new WaitForSeconds(text03DelayAfterImage02);
        text03.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInText(text03));

        continueButton.gameObject.SetActive(true);
        isSelected = false;
    }

    IEnumerator FadeOutText(TMP_Text textComponent)
    {
        float elapsedTime = 0;
        Color originalColor = textComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        textComponent.gameObject.SetActive(false);
    }

    IEnumerator FadeInText(TMP_Text textComponent)
    {
        float elapsedTime = 0;
        Color originalColor = textComponent.color;
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        textComponent.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator FadeInImage(Image image)
    {
        image.gameObject.SetActive(true);
        float elapsedTime = 0;
        Color originalColor = image.color;
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator FadeOutImage(Image image)
    {
        float elapsedTime = 0;
        Color originalColor = image.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    public void OnContinueButtonClick()
    {
        if (isFirstClick)
        {
            isFirstClick = false;
            StartCoroutine(FadeOutAndLoadNextSequence());
        }
        else
        {
            StartCoroutine(FadeInAndLoadNextScene());
        }
    }

    IEnumerator FadeInAndLoadNextScene()
    {
        fadeInImage.gameObject.SetActive(true);
        float elapsedTime = 0;
        Color originalColor = fadeInImage.color;
        fadeInImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeInImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        fadeInImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
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
        if (!isSelected)
        {
            button.transform.localScale = originalScale * hoverScale;
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
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

    public void OnPointerExit(Button button)
    {
        if (!isSelected)
        {
            button.transform.localScale = originalScale;
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.color = originalTextColor;
            }
        }
    }

    public void OnPointerClick(Button button)
    {
        isSelected = true;
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
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
