using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    private Animator animator;
    private int currentFillLevel = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IncreaseFillLevel()
    {
        currentFillLevel++;
        animator.SetInteger("FillLevel", currentFillLevel);
    }
}
