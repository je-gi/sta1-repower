using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    public Button germanButton;
    public Button englishButton;

    void Start()
    {
        germanButton.onClick.AddListener(() => SelectLanguage("de"));
        englishButton.onClick.AddListener(() => SelectLanguage("en"));
    }

    void SelectLanguage(string language)
    {
        LanguageManager.instance.SetLanguage(language);
    }
}
