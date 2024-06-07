using UnityEngine;
using UnityEngine.Video;

public class VideoPanelManager : MonoBehaviour
{
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnded;
    }

    public void OpenVideoPanel()
    {
        videoPanel.SetActive(true);
        videoPlayer.Play();
        MusicManager.Instance.SetVolumeForVideoPanel(true);
    }

    public void CloseVideoPanel()
    {
        videoPanel.SetActive(false);
        videoPlayer.Stop();
        MusicManager.Instance.SetVolumeForVideoPanel(false);
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        CloseVideoPanel();
    }
}
