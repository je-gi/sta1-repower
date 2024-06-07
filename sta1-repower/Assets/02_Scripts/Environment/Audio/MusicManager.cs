using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    public AudioSource audioSource;
    private float defaultVolume = 1.0f;
    private float reducedVolume = 0.1f;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource.volume = defaultVolume; 
    }

    public void SetVolumeForVideoPanel(bool isVideoPanelOpen)
    {
        audioSource.volume = isVideoPanelOpen ? reducedVolume : defaultVolume;
    }
}
