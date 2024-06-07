using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class CharacterPersonaManager : MonoBehaviour
{
    public List<GameObject> largePortraitObjects; 
    private int selectedIndex;

    void Start()
    {
        selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        if (selectedIndex >= 0 && selectedIndex < largePortraitObjects.Count)
        {
            ActivateLargePortrait(selectedIndex);
        }
    }

    void ActivateLargePortrait(int index)
    {
        foreach (var obj in largePortraitObjects)
        {
            obj.SetActive(false);
        }

        largePortraitObjects[index].SetActive(true);
        Animator animator = largePortraitObjects[index].GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsInNewScene", true);
        }
    }
}
