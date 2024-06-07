using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutImages : MonoBehaviour
{
    public Image overlayImage1;
    public Image overlayImage2;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0;

        SetAlpha(overlayImage1, 1);
        SetAlpha(overlayImage2, 1);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));

            SetAlpha(overlayImage1, alpha);
            SetAlpha(overlayImage2, alpha);

            yield return null;
        }

        SetAlpha(overlayImage1, 0);
        SetAlpha(overlayImage2, 0);
        overlayImage1.gameObject.SetActive(false);
        overlayImage2.gameObject.SetActive(false);
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
