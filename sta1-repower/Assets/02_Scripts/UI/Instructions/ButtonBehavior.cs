using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.1f;
    public Image fadeInImage;
    public float fadeDuration = 1f;

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
        StartCoroutine(FadeInImageAndLoadScene());
    }

    IEnumerator FadeInImageAndLoadScene()
    {
        if (fadeInImage != null)
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
        }

        yield return new WaitForSeconds(1f);

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
}
