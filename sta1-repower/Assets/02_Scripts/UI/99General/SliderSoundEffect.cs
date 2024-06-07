using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderSoundEffects : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource handleGrabAudioSource;
    public AudioSource handleReleaseAudioSource;

    private bool isDragging = false;

    void Start()
    {
        Slider slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        if (handleGrabAudioSource != null)
        {
            handleGrabAudioSource.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (handleReleaseAudioSource != null)
        {
            handleReleaseAudioSource.Play();
        }
    }

    void OnValueChanged(float value)
    {
        
    }
}
