using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionManager : MonoBehaviour
{
    public List<Image> characterImages;
    public List<GameObject> largePortraitObjects;
    public Button nextSceneButton;
    public AudioSource hoverAudioSource;
    public AudioSource submitAudioSource;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public float hoverScale = 1.05f;
    public TMP_Text nextSceneButtonText;

    private int currentIndex = -1;
    private Vector3[] originalScales;
    private Vector3 buttonOriginalScale;
    private bool isKeyboardControl = false;
    private int selectedIndex = -1;
    private Color originalButtonTextColor;

    void Start()
    {
        originalScales = new Vector3[characterImages.Count];
        buttonOriginalScale = nextSceneButton.transform.localScale;
        originalButtonTextColor = nextSceneButtonText.color;

        for (int i = 0; i < characterImages.Count; i++)
        {
            int index = i;
            originalScales[i] = characterImages[i].rectTransform.localScale;

            Animator animator = characterImages[i].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsSelected", false);
            }

            EventTrigger trigger = characterImages[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnter.callback.AddListener((eventData) => OnHoverEnter(index));
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExit.callback.AddListener((eventData) => OnHoverExit(index));
            trigger.triggers.Add(entryExit);

            EventTrigger.Entry entryClick = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            entryClick.callback.AddListener((eventData) => OnClick(index));
            trigger.triggers.Add(entryClick);
        }

        EventTrigger buttonTrigger = nextSceneButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry buttonEntryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        buttonEntryEnter.callback.AddListener((eventData) => OnButtonHoverEnter());
        buttonTrigger.triggers.Add(buttonEntryEnter);

        EventTrigger.Entry buttonEntryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        buttonEntryExit.callback.AddListener((eventData) => OnButtonHoverExit());
        buttonTrigger.triggers.Add(buttonEntryExit);

        nextSceneButton.onClick.AddListener(OnNextSceneButtonClick);

        SelectCharacter(0);
        HighlightCharacter(0);
        SelectCurrentCharacter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isKeyboardControl = true;
            Navigate(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isKeyboardControl = true;
            Navigate(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            isKeyboardControl = true;
            Navigate(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            isKeyboardControl = true;
            Navigate(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (currentIndex == 6)
            {
                nextSceneButton.onClick.Invoke();
            }
            else
            {
                SelectCurrentCharacter();
            }
        }
    }

    void OnMouseMove()
    {
        isKeyboardControl = false;
        if (currentIndex != -1)
        {
            ResetCharacter(currentIndex);
            currentIndex = -1;
        }
    }

    public void OnHoverEnter(int index)
    {
        if (!isKeyboardControl)
        {
            HighlightCharacter(index);
            if (hoverAudioSource != null)
            {
                hoverAudioSource.Play();
            }
        }
    }

    public void OnHoverExit(int index)
    {
        if (!isKeyboardControl && index != currentIndex && index != selectedIndex)
        {
            ResetCharacter(index);
        }
    }

    public void OnClick(int index)
    {
        isKeyboardControl = false;
        SelectCharacter(index);
        SelectCurrentCharacter();
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }
    }

    public void OnButtonHoverEnter()
    {
        nextSceneButton.transform.localScale = buttonOriginalScale * hoverScale;
        nextSceneButtonText.color = hoverColor;
        if (hoverAudioSource != null)
        {
            hoverAudioSource.Play();
        }
    }

    public void OnButtonHoverExit()
    {
        nextSceneButton.transform.localScale = buttonOriginalScale;
        nextSceneButtonText.color = originalButtonTextColor;
    }

    public void HighlightCharacter(int index)
    {
        characterImages[index].rectTransform.localScale = originalScales[index] * hoverScale;
        if (index == selectedIndex)
        {
            Animator animator = characterImages[index].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsSelected", true);
            }
        }
    }

    public void ResetCharacter(int index)
    {
        characterImages[index].rectTransform.localScale = originalScales[index];
        if (index != selectedIndex)
        {
            Animator animator = characterImages[index].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsSelected", false);
            }
        }
    }

    public void SelectCharacter(int index)
    {
        for (int i = 0; i < characterImages.Count; i++)
        {
            if (i == index)
            {
                HighlightCharacter(i);
            }
            else
            {
                ResetCharacter(i);
            }
        }

        if (index >= 0 && index < characterImages.Count)
        {
            UpdateLargePortrait(index);
        }
        else
        {
            HideAllLargePortraits();
        }

        currentIndex = index;

        if (currentIndex != 6)
        {
            OnButtonHoverExit();
        }
    }

    public void SelectCurrentCharacter()
    {
        selectedIndex = currentIndex;
        for (int i = 0; i < characterImages.Count; i++)
        {
            if (i == selectedIndex)
            {
                Animator animator = characterImages[i].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("IsSelected", true);
                }
            }
            else
            {
                Animator animator = characterImages[i].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("IsSelected", false);
                }
            }
            ResetCharacter(i);
        }
    }

    public void UpdateLargePortrait(int index)
    {
        HideAllLargePortraits();
        if (index >= 0 && index < largePortraitObjects.Count)
        {
            largePortraitObjects[index].SetActive(true);
        }
    }

    private void HideAllLargePortraits()
    {
        foreach (var obj in largePortraitObjects)
        {
            obj.SetActive(false);
        }
    }

    private void Navigate(int colChange, int rowChange)
    {
        if (currentIndex == -1)
        {
            currentIndex = 0;
            SelectCharacter(currentIndex);
            return;
        }

        int rows = 3;
        int cols = 2;
        int currentCol = currentIndex / rows;
        int currentRow = currentIndex % rows;

        int newCol = (currentCol + colChange + cols) % cols;
        int newRow = (currentRow + rowChange + rows) % rows;

        int newIndex = newCol * rows + newRow;

        if (currentIndex == 5 && colChange == 1)
        {
            ResetCharacter(currentIndex);
            currentIndex = 6;
            HighlightButton();
        }
        else if (currentIndex == 6 && colChange == -1)
        {
            OnButtonHoverExit();
            currentIndex = 5;
            HighlightCharacter(currentIndex);
        }
        else if (currentIndex == 6 && colChange == 1)
        {
            OnButtonHoverExit();
            currentIndex = 5;
            HighlightCharacter(currentIndex);
        }
        else
        {
            SelectCharacter(newIndex);
        }
    }

    public void OnNextSceneButtonClick()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedIndex);
        StartCoroutine(PlayAnimationAndLoadScene());
    }

    private IEnumerator PlayAnimationAndLoadScene()
    {
        if (submitAudioSource != null)
        {
            submitAudioSource.Play();
        }

        Animator largePortraitAnimator = largePortraitObjects[selectedIndex].GetComponent<Animator>();
        if (largePortraitAnimator != null)
        {
            largePortraitAnimator.SetBool("IsSelected", true);
            yield return new WaitForSeconds(1.7f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void HighlightButton()
    {
        nextSceneButton.transform.localScale = buttonOriginalScale * hoverScale;
        nextSceneButtonText.color = hoverColor;
    }
}
