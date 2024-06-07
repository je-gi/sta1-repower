using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public GameObject background;
    public Button germanButton;
    public Button englishButton;
    public TMP_Text blinkingText;
    public GameObject titleObject;
    public float blinkSpeed = 1.0f;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public AudioSource startGameAudioSource;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Animator backgroundAnimator;
    public float titleFadeDuration = 0.5f;
    public float delayBeforeBlinkingText = 1.0f;

    private Color originalButtonColor;
    private Color originalTextColor;
    private Vector3 originalButtonScale;
    private Vector3 originalTextScale;
    private bool inputEnabled = false;
    private bool languageSelected = false;

    void Start()
    {
        germanButton.onClick.AddListener(() => OnLanguageButtonClick("de"));
        englishButton.onClick.AddListener(() => OnLanguageButtonClick("en"));

        originalButtonColor = germanButton.image.color;
        originalButtonScale = germanButton.transform.localScale;
        originalTextColor = blinkingText.color;
        originalTextScale = blinkingText.transform.localScale;

        blinkingText.gameObject.SetActive(false);

        AddEventTriggers(germanButton);
        AddEventTriggers(englishButton);
    }

    void Update()
    {
        if (inputEnabled && Input.anyKeyDown)
        {
            inputEnabled = false;
            startGameAudioSource.Play();
            StartCoroutine(FadeOutAndLoadScene());
        }

        if (!languageSelected)
        {
            if (EventSystem.current.currentSelectedGameObject == germanButton.gameObject)
            {
                OnPointerEnter(germanButton);
            }
            else if (EventSystem.current.currentSelectedGameObject == englishButton.gameObject)
            {
                OnPointerEnter(englishButton);
            }
            else
            {
                OnPointerExit(germanButton);
                OnPointerExit(englishButton);
            }
        }
    }

    void OnLanguageButtonClick(string language)
    {
        if (!languageSelected)
        {
            languageSelected = true;
            submitAudioSource.Play();
            SelectLanguage(language);
            StartCoroutine(FadeOutButtonsAndShowBlinkingText());
        }
    }

    void SelectLanguage(string language)
    {
        LanguageManager.instance.SetLanguage(language);
    }

    IEnumerator FadeOutAndLoadScene()
    {
        backgroundAnimator.SetTrigger("StartAnimation");
        StartCoroutine(FadeOutTitleObject());
        yield return StartCoroutine(FadeOutUIElements());
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeOutTitleObject()
    {
        float elapsedTime = 0;
        CanvasGroup canvasGroup = titleObject.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = titleObject.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < titleFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / titleFadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0;
        titleObject.SetActive(false);
    }

    IEnumerator FadeOutUIElements()
    {
        float elapsedTime = 0;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));

            SetAlpha(germanButton.image, alpha);
            SetAlpha(englishButton.image, alpha);
            SetAlpha(blinkingText, alpha);

            yield return null;
        }

        SetAlpha(germanButton.image, 0);
        SetAlpha(englishButton.image, 0);
        SetAlpha(blinkingText, 0);

        germanButton.gameObject.SetActive(false);
        englishButton.gameObject.SetActive(false);
        blinkingText.gameObject.SetActive(false);
    }

    IEnumerator FadeOutButtonsAndShowBlinkingText()
    {
        float elapsedTime = 0;
        float fadeDuration = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));

            SetAlpha(germanButton.image, alpha);
            SetAlpha(englishButton.image, alpha);

            yield return null;
        }

        SetAlpha(germanButton.image, 0);
        SetAlpha(englishButton.image, 0);

        germanButton.gameObject.SetActive(false);
        englishButton.gameObject.SetActive(false);

        hoverAudioSource.Stop();
        submitAudioSource.Stop();
        hoverAudioSource.mute = true;
        submitAudioSource.mute = true;

        yield return new WaitForSeconds(delayBeforeBlinkingText);

        blinkingText.gameObject.SetActive(true);
        StartBlinking();
        inputEnabled = true;
    }

    void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    void StartBlinking()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkingText.color = originalTextColor;
            yield return new WaitForSeconds(blinkSpeed);
            blinkingText.color = Color.clear;
            yield return new WaitForSeconds(blinkSpeed);
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
        if (!languageSelected)
        {
            button.transform.localScale = originalButtonScale * 1.1f;
            button.image.color = hoverColor;
            hoverAudioSource.Play();
        }
    }

    public void OnPointerExit(Button button)
    {
        if (!languageSelected)
        {
            button.transform.localScale = originalButtonScale;
            button.image.color = originalButtonColor;
        }
    }

    public void OnPointerClick(Button button)
    {
        if (!languageSelected)
        {
            button.image.color = selectedColor;
            OnLanguageButtonClick(button == germanButton ? "de" : "en");
        }
    }
}