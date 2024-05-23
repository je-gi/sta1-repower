using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionnaireManager : MonoBehaviour
{
    public Slider ageSlider;
    public TMP_Text ageText;
    public TMP_Dropdown dropdownQuestion;
    public Slider answerSlider;
    public TMP_Text answerText;
    public Button nextButton;

    void Start()
    {
        nextButton.interactable = false;

        ageSlider.onValueChanged.AddListener(delegate { UpdateAgeText(); });
        dropdownQuestion.onValueChanged.AddListener(delegate { ValidateInputs(); });
        answerSlider.onValueChanged.AddListener(delegate { UpdateAnswerText(); });

        UpdateAgeText();
        UpdateAnswerText();
    }

    void UpdateAgeText()
    {
        ageText.text = "Alter: " + ageSlider.value.ToString("0");
        ValidateInputs();
    }

    void UpdateAnswerText()
    {
        answerText.text = "Selbsteinschätzung: " + answerSlider.value.ToString("0");
        ValidateInputs();
    }

    void ValidateInputs()
    {
        if (ageSlider.value > 0 && dropdownQuestion.value > 0 && answerSlider.value > 0)
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
    }

    public void OnNextButtonClicked()
    {
        SceneManager.LoadScene("Overview");
}
}
