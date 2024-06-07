using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseHover : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public string sceneToLoad;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
