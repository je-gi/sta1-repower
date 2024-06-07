using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HouseHover : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
        if (hoverAudioSource != null)
        {
            hoverAudioSource.Play();
        }
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    void OnMouseDown()
    {
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
        StartCoroutine(LoadSceneAfterSound());
    }

    private IEnumerator LoadSceneAfterSound()
    {
        if (submitAudioSource != null)
        {
            yield return new WaitForSeconds(submitAudioSource.clip.length);
        }
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
