using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject group1;
    public GameObject group2;
    public Button continueButton;
    private bool firstClick = false;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClick);
        group1.SetActive(true);
        group2.SetActive(false);
        originalScale = continueButton.transform.localScale;
        originalColor = continueButton.image.color; // Assuming the Button has an Image component
    }

    void OnContinueButtonClick()
    {
        if (!firstClick)
        {
            group1.SetActive(false);
            group2.SetActive(true);
            firstClick = true;
        }
        else
        {
            SceneManager.LoadScene("Overview");
        }
    }

    void Update()
    {
        // Check if the continueButton is the selected button and scale it up
        if (EventSystem.current.currentSelectedGameObject == continueButton.gameObject)
        {
            continueButton.transform.localScale = originalScale * 1.1f;
        }
        else
        {
            continueButton.transform.localScale = originalScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        continueButton.transform.localScale = originalScale * 1.1f;
        continueButton.image.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        continueButton.transform.localScale = originalScale;
        continueButton.image.color = originalColor;
    }
}
