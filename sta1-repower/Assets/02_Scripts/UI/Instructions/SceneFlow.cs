using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SequentialTextAndImageManager : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public Image image1;
    public Image image2;
    public Button continueButton;

    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 1f;
    public float text2Delay = 1f;
    public float image1DisplayDelay = 0.5f;
    public float image2DisplayDelay = 0.5f;
    public float text3Delay = 1f;
    public float buttonDisplayDelay = 2f;

    private string text1Content;
    private string text2Content;
    private string text3Content;

    private bool isText3Complete = false;

    void Start()
    {
        text1Content = text1.text;
        text2Content = text2.text;
        text3Content = text3.text;

        text1.text = "";
        text2.text = "";
        text3.text = "";

        SetAlpha(image1, 0);
        SetAlpha(image2, 0);
        continueButton.gameObject.SetActive(false);

        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        yield return StartCoroutine(DisplayTextWithTypewriter(text1, text1Content));
        yield return StartCoroutine(FadeOutText(text1));
        yield return new WaitForSeconds(text2Delay);
        yield return StartCoroutine(DisplayTextWithTypewriter(text2, text2Content));
        yield return new WaitForSeconds(image1DisplayDelay);
        yield return StartCoroutine(FadeInImage(image1));
        yield return new WaitForSeconds(image2DisplayDelay);
        yield return StartCoroutine(FadeInImage(image2));
        yield return new WaitForSeconds(text3Delay);
        yield return StartCoroutine(DisplayTextWithTypewriter(text3, text3Content));
        StartCoroutine(ShowContinueButton());
    }

    IEnumerator DisplayTextWithTypewriter(TMP_Text textComponent, string textContent)
    {
        foreach (char c in textContent)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        isText3Complete = true; // Markiere den Text als komplett angezeigt
    }

    IEnumerator FadeOutText(TMP_Text textComponent)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            textComponent.alpha = alpha;
            yield return null;
        }
        textComponent.alpha = 0;
    }

    IEnumerator FadeInImage(Image image)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(image, alpha);
            yield return null;
        }
        SetAlpha(image, 1);
    }

    IEnumerator ShowContinueButton()
    {
        yield return new WaitForSeconds(buttonDisplayDelay);
        continueButton.gameObject.SetActive(true);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    void OnContinueButtonClicked()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        yield return StartCoroutine(FadeOutText(text1));
        yield return StartCoroutine(FadeOutText(text2));
        yield return StartCoroutine(FadeOutText(text3));
        yield return StartCoroutine(FadeOutImage(image1));
        yield return StartCoroutine(FadeOutImage(image2));
        continueButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeOutImage(Image image)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            SetAlpha(image, alpha);
            yield return null;
        }
        SetAlpha(image, 0);
    }

    void SetAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
