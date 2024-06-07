using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class QuestionnaireManager : MonoBehaviour
{
    public Slider ageSlider;
    public TMP_Text ageText;
    public TMP_Dropdown dropdownQuestion;

    public Button buttonLow;
    public Button buttonMedium;
    public Button buttonHigh;

    public Button buttonMorning;
    public Button buttonDay;
    public Button buttonEvening;

    public Button buttonCanHear;
    public Button buttonCannotHear;

    public GameObject group1;
    public GameObject group2;

    private int selfAssessment = 0;
    private int timeOfDay = -1;
    private bool canHear = false;
    private bool firstSetComplete = false;

    void Start()
    {
        group2.SetActive(false);

        ageSlider.onValueChanged.AddListener(delegate { UpdateAgeText(); });
        dropdownQuestion.onValueChanged.AddListener(delegate { ValidateInputs(); });

        buttonLow.onClick.AddListener(delegate { SetSelfAssessment(-1); });
        buttonMedium.onClick.AddListener(delegate { SetSelfAssessment(0); });
        buttonHigh.onClick.AddListener(delegate { SetSelfAssessment(1); });

        buttonMorning.onClick.AddListener(delegate { SetTimeOfDay(0); });
        buttonDay.onClick.AddListener(delegate { SetTimeOfDay(1); });
        buttonEvening.onClick.AddListener(delegate { SetTimeOfDay(2); });

        buttonCanHear.onClick.AddListener(delegate { SetCanHear(true); });
        buttonCannotHear.onClick.AddListener(delegate { SetCanHear(false); });

        UpdateAgeText();
    }

    void UpdateAgeText()
    {
        ageText.text = "age: " + ageSlider.value.ToString("0");
        ValidateInputs();
    }

    void SetSelfAssessment(int value)
    {
        selfAssessment = value;
        HighlightButton(value);
        ValidateInputs();
    }

    void SetTimeOfDay(int value)
    {
        timeOfDay = value;
        HighlightTimeButton(value);
        ValidateInputs();
    }

    void SetCanHear(bool value)
    {
        canHear = value;
        HighlightHearingButton(value);
        ValidateInputs();
    }

    void HighlightButton(int value)
    {
        buttonLow.transform.localScale = value == -1 ? Vector3.one * 1.1f : Vector3.one;
        buttonMedium.transform.localScale = value == 0 ? Vector3.one * 1.1f : Vector3.one;
        buttonHigh.transform.localScale = value == 1 ? Vector3.one * 1.1f : Vector3.one;
    }

    void HighlightTimeButton(int value)
    {
        buttonMorning.transform.localScale = value == 0 ? Vector3.one * 1.1f : Vector3.one;
        buttonDay.transform.localScale = value == 1 ? Vector3.one * 1.1f : Vector3.one;
        buttonEvening.transform.localScale = value == 2 ? Vector3.one * 1.1f : Vector3.one;
    }

    void HighlightHearingButton(bool value)
    {
        buttonCanHear.transform.localScale = value ? Vector3.one * 1.1f : Vector3.one;
        buttonCannotHear.transform.localScale = !value ? Vector3.one * 1.1f : Vector3.one;
    }


    void ValidateInputs()
    {
    }
}
