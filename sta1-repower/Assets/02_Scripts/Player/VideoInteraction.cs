using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class VideoInteraction : MonoBehaviour
{
    public GameObject videoPanel1;
    public GameObject videoPanel2;
    public GameObject textPanel;
    public GameObject videoPanel4;
    public RawImage videoDisplay1;
    public RawImage videoDisplay2;
    public RawImage videoDisplay4;
    public Button playButton1;
    public Button stopButton1;
    public Button restartButton1;
    public Button closeButton1;
    public Button playButton2;
    public Button stopButton2;
    public Button restartButton2;
    public Button closeButton2;
    public Button closeButton3;
    public Button playButton4;
    public Button stopButton4;
    public Button restartButton4;
    public Button closeButton4;
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;
    public VideoPlayer videoPlayer4;
    public KeyCode interactionKey = KeyCode.F;
    private Collider2D currentSprite;
    public List<GameObject> initialSprites;
    public GameObject subsequentSprite;
    public GameObject finalSprite;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;
    public float fadeDuration = 1f;
    public GameObject player;
    public BatteryManager batteryManager;
    public Animator screenAnimator; 

    void Start()
    {
        playButton1.onClick.AddListener(() => PlayVideo(videoPlayer1));
        stopButton1.onClick.AddListener(() => StopVideo(videoPlayer1));
        restartButton1.onClick.AddListener(() => RestartVideo(videoPlayer1));
        closeButton1.onClick.AddListener(() => CloseVideoPanel(videoPanel1, 1));

        playButton2.onClick.AddListener(() => PlayVideo(videoPlayer2));
        stopButton2.onClick.AddListener(() => StopVideo(videoPlayer2));
        restartButton2.onClick.AddListener(() => RestartVideo(videoPlayer2));
        closeButton2.onClick.AddListener(() => CloseVideoPanel(videoPanel2, 2));

        closeButton3.onClick.AddListener(() => CloseTextPanel());

        playButton4.onClick.AddListener(() => PlayVideo(videoPlayer4));
        stopButton4.onClick.AddListener(() => StopVideo(videoPlayer4));
        restartButton4.onClick.AddListener(() => RestartVideo(videoPlayer4));
        closeButton4.onClick.AddListener(() => CloseVideoPanel(videoPanel4, 4));

        videoPanel1.SetActive(false);
        videoPanel2.SetActive(false);
        textPanel.SetActive(false);
        videoPanel4.SetActive(false);
        videoPlayer1.prepareCompleted += PrepareCompleted;
        videoPlayer2.prepareCompleted += PrepareCompleted;
        videoPlayer4.prepareCompleted += PrepareCompleted;

        ActivateInitialSprites();
        SetCanvasSortingOrder();
    }

    void Update()
    {
        if (currentSprite != null && Input.GetKeyDown(interactionKey))
        {
            StartCoroutine(OpenPanelWithFade(currentSprite.gameObject));
        }

        if (Input.GetMouseButtonDown(0) && currentSprite != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (currentSprite.OverlapPoint(mousePosition))
            {
                StartCoroutine(OpenPanelWithFade(currentSprite.gameObject));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentSprite = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (currentSprite == other)
            {
                currentSprite = null;
            }
        }
    }

    IEnumerator OpenPanelWithFade(GameObject sprite)
    {
        yield return StartCoroutine(FadeOutSpriteAndChildren(sprite));
        OpenPanel(sprite);
    }

    IEnumerator FadeOutSpriteAndChildren(GameObject sprite)
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(sprite.GetComponentsInChildren<SpriteRenderer>());
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            }
            yield return null;
        }
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            spriteRenderer.gameObject.SetActive(false);
        }
        sprite.SetActive(false);
    }

    void OpenPanel(GameObject sprite)
    {
        DisableAllSpritesAndChildren();
        if (sprite == initialSprites[0])
        {
            videoPanel1.SetActive(true);
            videoPlayer1.gameObject.SetActive(true);
            videoPlayer1.Prepare();
        }
        else if (sprite == initialSprites[1])
        {
            videoPanel2.SetActive(true);
            videoPlayer2.gameObject.SetActive(true);
            videoPlayer2.Prepare();
        }
        else if (sprite == subsequentSprite)
        {
            textPanel.SetActive(true);
        }
        else if (sprite == finalSprite)
        {
            videoPanel4.SetActive(true);
            videoPlayer4.gameObject.SetActive(true);
            videoPlayer4.Prepare();
        }
    }

    void CloseVideoPanel(GameObject videoPanel, int panelNumber)
    {
        videoPanel.SetActive(false);
        if (videoPanel == videoPanel1)
        {
            videoPlayer1.Stop();
            videoPlayer1.gameObject.SetActive(false);
        }
        else if (videoPanel == videoPanel2)
        {
            videoPlayer2.Stop();
            videoPlayer2.gameObject.SetActive(false);
        }
        else if (videoPanel == videoPanel4)
        {
            videoPlayer4.Stop();
            videoPlayer4.gameObject.SetActive(false);
        }

        batteryManager.IncreaseFillLevel();

        if (panelNumber == 1 || panelNumber == 2)
        {
            subsequentSprite.SetActive(true);
            canvas3.SetActive(true);
        }

        if (panelNumber == 4)
        {
            StartCoroutine(TriggerScreenEffectsAndLoadScene());
        }
    }

    void CloseTextPanel()
    {
        textPanel.SetActive(false);
        subsequentSprite.SetActive(false);
        finalSprite.SetActive(true);
        canvas4.SetActive(true);
        batteryManager.IncreaseFillLevel();
    }

    void PlayVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Play();
    }

    void StopVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Pause();
    }

    void RestartVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }

    void ActivateInitialSprites()
    {
        SetActiveSprites(true, initialSprites);
        if (subsequentSprite != null)
        {
            subsequentSprite.SetActive(false);
            canvas3.SetActive(false);
        }
        if (finalSprite != null)
        {
            finalSprite.SetActive(false);
            canvas4.SetActive(false);
        }
    }

    void SetActiveSprites(bool active, List<GameObject> sprites)
    {
        foreach (var sprite in sprites)
        {
            sprite.SetActive(active);
            foreach (Transform child in sprite.transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }

    void PrepareCompleted(VideoPlayer vp)
    {
        vp.Play();
        vp.Pause();
    }

    void SetCanvasSortingOrder()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (var canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.sortingOrder = -1;
            }
        }

        Canvas playerCanvas = player.GetComponent<Canvas>();
        if (playerCanvas != null)
        {
            playerCanvas.sortingOrder = 0;
        }

        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sortingOrder = 0;
        }
    }

    void DisableAllSpritesAndChildren()
    {
        SetActiveSprites(false, initialSprites);
        subsequentSprite.SetActive(false);
        finalSprite.SetActive(false);
    }

    IEnumerator TriggerScreenEffectsAndLoadScene()
    {
        yield return new WaitForSeconds(1.7f); 
        screenAnimator.SetBool("IsFlickering", true);
        yield return new WaitForSeconds(screenAnimator.GetCurrentAnimatorStateInfo(0).length);
        screenAnimator.SetBool("IsFlickering", false);
        screenAnimator.SetBool("IsBlackout", true);
        yield return new WaitForSeconds(screenAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}