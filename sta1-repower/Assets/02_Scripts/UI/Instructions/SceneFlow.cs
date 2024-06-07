using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneFlowManager : MonoBehaviour
{
    public TMP_Text text01;
    public TMP_Text text02;
    public TMP_Text text03;
    public Image image01;
    public Image image02;
    public Button continueButton;
    public Image fadeInImage;
    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 1f;
    public float text03DelayAfterImage02 = 3f;

    private string text01Content;

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

        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        yield return StartCoroutine(FadeOutImage(fadeInImage));
        fadeInImage.gameObject.SetActive(false);

        yield return StartCoroutine(DisplayTextWithTypewriter(text01, text01Content));
        yield return StartCoroutine(FadeOutText(text01));

        yield return new WaitForSeconds(2f);
        text02.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInText(text02));

        yield return StartCoroutine(FadeInImage(image01));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeInImage(image02));

        yield return new WaitForSeconds(text03DelayAfterImage02);
        text03.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInText(text03));

        continueButton.gameObject.SetActive(true);
    }

    IEnumerator DisplayTextWithTypewriter(TMP_Text textComponent, string textContent)
    {
        foreach (char c in textContent)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
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
}
