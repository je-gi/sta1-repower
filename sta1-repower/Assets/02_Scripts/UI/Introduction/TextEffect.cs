using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextEffects : MonoBehaviour
{
    public TMP_Text quoteText;
    public TMP_Text mainText;
    public Image lightningImage;
    public Image backgroundImage1;
    public Image backgroundImage2;
    public Animator lightningAnimator;
    public Animator backgroundAnimator;
    public Button continueButton;

    public float imageDisplayTime = 4f;
    public float quoteDisplayTime = 5f;
    public float fadeDuration = 1f;
    public float typewriterSpeed = 0.05f;
    public float mainTextDelay = 2f;
    public float buttonDisplayDelay = 2f;

    private bool isQuoteComplete = false;
    private bool isMainTextComplete = false;
    private bool isInputActive = false;
    private string mainTextContent;

    void Start()
    {
        mainTextContent = mainText.text;
        mainText.text = "";
        quoteText.alpha = 0;
        lightningImage.canvasRenderer.SetAlpha(0);
        backgroundImage1.canvasRenderer.SetAlpha(0);
        backgroundImage2.canvasRenderer.SetAlpha(0);
        continueButton.gameObject.SetActive(false);
        StartCoroutine(DisplaySequence());
    }

    void Update()
    {
        if (isInputActive && !isMainTextComplete && Input.anyKeyDown)
        {
            StopAllCoroutines();
            mainText.text = mainTextContent;
            isMainTextComplete = true;
            StartCoroutine(ShowContinueButton());
        }
    }

    IEnumerator DisplaySequence()
    {
        yield return StartCoroutine(FadeInImage(lightningImage));
        yield return new WaitForSeconds(imageDisplayTime);
        yield return StartCoroutine(DisplayQuote());
        yield return new WaitForSeconds(quoteDisplayTime);
        StartCoroutine(FadeOutQuoteAndChangeImageAnimation());
    }

    IEnumerator FadeInImage(Image image)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }
        image.canvasRenderer.SetAlpha(1);
    }

    IEnumerator FadeOutImage(Image image)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            image.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }
        image.canvasRenderer.SetAlpha(0);
    }

    IEnumerator DisplayQuote()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            quoteText.alpha = alpha;
            yield return null;
        }
        quoteText.alpha = 1;
    }

    IEnumerator FadeOutQuoteAndChangeImageAnimation()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            quoteText.alpha = alpha;
            yield return null;
        }
        quoteText.alpha = 0;
        isQuoteComplete = true;

        lightningAnimator.SetBool("IsStarting", true);

        yield return new WaitForSeconds(mainTextDelay);
        StartCoroutine(FadeOutLightningAndDisplayMainText());
    }

    IEnumerator FadeOutLightningAndDisplayMainText()
    {
        yield return new WaitForSeconds(mainTextDelay);
        backgroundImage1.canvasRenderer.SetAlpha(1);
        backgroundImage2.canvasRenderer.SetAlpha(1);
        StartCoroutine(FadeOutImage(lightningImage));
        yield return new WaitForSeconds(fadeDuration);
        StartCoroutine(DisplayMainText());
    }

    IEnumerator DisplayMainText()
    {
        mainText.gameObject.SetActive(true);
        isInputActive = true;
        foreach (char c in mainTextContent)
        {
            mainText.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        isMainTextComplete = true;
        isInputActive = false;
        StartCoroutine(ShowContinueButton());
    }

    IEnumerator ShowContinueButton()
    {
        yield return new WaitForSeconds(buttonDisplayDelay);
        continueButton.gameObject.SetActive(true);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    void OnContinueButtonClicked()
    {
        StartCoroutine(FadeOutUIElementsAndStartBackgroundAnimation());
    }

    IEnumerator FadeOutUIElementsAndStartBackgroundAnimation()
    {
        float elapsedTime = 0;
        CanvasGroup mainTextCanvasGroup = mainText.GetComponent<CanvasGroup>();
        CanvasGroup continueButtonCanvasGroup = continueButton.GetComponent<CanvasGroup>();

        if (mainTextCanvasGroup == null)
            mainTextCanvasGroup = mainText.gameObject.AddComponent<CanvasGroup>();

        if (continueButtonCanvasGroup == null)
            continueButtonCanvasGroup = continueButton.gameObject.AddComponent<CanvasGroup>();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            mainTextCanvasGroup.alpha = alpha;
            continueButtonCanvasGroup.alpha = alpha;
            yield return null;
        }
        mainTextCanvasGroup.alpha = 0;
        continueButtonCanvasGroup.alpha = 0;

        backgroundAnimator.SetBool("IsStarting", true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FastForwardToMainText()
    {
        quoteText.alpha = 0;
        isQuoteComplete = true;
        lightningAnimator.SetBool("IsStarting", true);
        yield return new WaitForSeconds(mainTextDelay);
        StartCoroutine(FadeOutLightningAndDisplayMainText());
        StartCoroutine(DisplayMainText());
    }
}
