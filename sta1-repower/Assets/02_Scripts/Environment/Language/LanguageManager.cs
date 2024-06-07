using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;
    private string selectedLanguage = "en";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(string language)
    {
        selectedLanguage = language;
        Debug.Log("Selected Language: " + selectedLanguage);
    }

    public string GetLanguage()
    {
        return selectedLanguage;
    }
}
