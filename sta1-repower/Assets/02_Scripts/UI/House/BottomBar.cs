using UnityEngine;

public class PanelAnimator : MonoBehaviour
{
    public Animator panelAnimator;
    private bool isUp = false;

    void Start()
    {
        if (panelAnimator == null)
        {
            panelAnimator = GetComponent<Animator>();
        }
        panelAnimator.SetBool("StartPosition", true);
    }

    public void TogglePanel()
    {
        if (isUp)
        {
            panelAnimator.SetBool("MoveDown", true);
            panelAnimator.SetBool("MoveUp", false);
            panelAnimator.SetBool("StartPosition", false);
        }
        else
        {
            panelAnimator.SetBool("MoveUp", true);
            panelAnimator.SetBool("MoveDown", false);
            panelAnimator.SetBool("StartPosition", false);
        }
        isUp = !isUp;
    }

    public void OnMoveDownComplete()
    {
        panelAnimator.SetBool("StartPosition", true);
        panelAnimator.SetBool("MoveDown", false);
    }
}
