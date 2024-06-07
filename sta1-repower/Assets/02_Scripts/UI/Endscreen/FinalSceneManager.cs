using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalSceneManager : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public SpriteRenderer batterySprite;
    public GameObject spriteContainer;
    public Animator spriteAnimator;
    public Animator batteryAnimator;
    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 1f;
    public float text2Delay = 1f;
    public float text3Delay = 2f;
    private string text1Content;
    private string text2Content;
    private string text3Content;
    private int currentStage = 0;
    private bool inputEnabled = false;

    void Start()
    {
        text1Content = text1.text;
        text2Content = text2.text;
        text3Content = text3.text;

        text1.text = "";
        text2.text = "";
        text3.text = "";

        SetAlpha(batterySprite, 1); 
        foreach (Transform child in spriteContainer.transform)
        {
            SetAlpha(child.GetComponent<SpriteRenderer>(), 0);
        }

        batterySprite.gameObject.SetActive(false);
        spriteContainer.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);

        StartCoroutine(SceneSequence());
    }

    void Update()
    {
        if (inputEnabled && Input.anyKeyDown)
        {
            currentStage++;
            if (currentStage <= 6)
            {
                spriteAnimator.SetInteger("Stage", currentStage);
                batteryAnimator.SetInteger("FillLevel", 6 - currentStage);
            }

            if (currentStage == 6)
            {
                StartCoroutine(FadeOutAndLoadScene());
            }
        }
    }

    IEnumerator SceneSequence()
    {
        text1.gameObject.SetActive(true);
        yield return StartCoroutine(DisplayTextWithTypewriter(text1, text1Content));
        yield return new WaitForSeconds(text2Delay);
        yield return StartCoroutine(FadeOutText(text1));

        text2.gameObject.SetActive(true);
        yield return StartCoroutine(DisplayTextWithTypewriter(text2, text2Content));
        yield return new WaitForSeconds(text3Delay);

        batterySprite.gameObject.SetActive(true);
        spriteContainer.SetActive(true);
        yield return StartCoroutine(FadeInUIElements());

        text3.gameObject.SetActive(true);
        yield return StartCoroutine(DisplayTextWithTypewriter(text3, text3Content));

        inputEnabled = true;
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

    IEnumerator FadeInUIElements()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            foreach (Transform child in spriteContainer.transform)
            {
                SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
                if (childSprite != null)
                {
                    childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, alpha);
                }
            }
            yield return null;
        }

        foreach (Transform child in spriteContainer.transform)
        {
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            if (childSprite != null)
            {
                childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, 1);
            }
        }
    }

    IEnumerator FadeOutAndLoadScene()
    {
        inputEnabled = false;

        yield return StartCoroutine(FadeOutText(text3));
        yield return StartCoroutine(FadeOutUIElements());

        SceneManager.LoadScene(0);
    }

    IEnumerator FadeOutUIElements()
    {
        float elapsedTime = 0;
        Color originalColor = batterySprite.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            batterySprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            foreach (Transform child in spriteContainer.transform)
            {
                SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
                if (childSprite != null)
                {
                    childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, alpha);
                }
            }
            yield return null;
        }

        batterySprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        foreach (Transform child in spriteContainer.transform)
        {
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            if (childSprite != null)
            {
                childSprite.color = new Color(childSprite.color.r, childSprite.color.g, childSprite.color.b, 0);
                child.gameObject.SetActive(false);
            }
        }

        batterySprite.gameObject.SetActive(false);
        spriteContainer.SetActive(false);
    }

    void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
