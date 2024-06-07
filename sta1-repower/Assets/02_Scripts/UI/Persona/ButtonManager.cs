using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    public GameObject group1;
    public GameObject group2;
    public Button continueButton;
    public TMP_Text buttonText;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.1f;
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private bool firstClick = false;
    private Vector3 originalScale;
    private Color originalTextColor;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
        group1.SetActive(true);
        group2.SetActive(false);
        originalScale = continueButton.transform.localScale;
        originalTextColor = buttonText.color;

        AddEventTriggers(continueButton);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    void OnContinueButtonClick()
    {
        if (!firstClick)
        {
            group1.SetActive(false);
            group2.SetActive(true);
            firstClick = true;
            ResetButtonState();
        }
        else
        {
            StartCoroutine(PlaySubmitSoundAndLoadScene());
        }
    }

    IEnumerator PlaySubmitSoundAndLoadScene()
    {
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
            yield return new WaitForSeconds(submitAudioSource.clip.length);
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); 
            yield return StartCoroutine(FadeInImage(fadeImage));
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in build settings!");
        }
    }

    IEnumerator FadeInImage(Image image)
    {
        float elapsedTime = 0f;
        Color originalColor = image.color;
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    void ResetButtonState()
    {
        continueButton.transform.localScale = originalScale;
        buttonText.color = originalTextColor;
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
        button.transform.localScale = originalScale * hoverScale;
        buttonText.color = hoverColor;
        if (hoverAudioSource != null)
        {
            hoverAudioSource.Play();
        }
    }

    public void OnPointerExit(Button button)
    {
        button.transform.localScale = originalScale;
        buttonText.color = originalTextColor;
    }

    public void OnPointerClick(Button button)
    {
        buttonText.color = selectedColor;
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
    }
}
