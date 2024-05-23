using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportTarget; // Ziehe das entsprechende TeleportTarget GameObject hier rein
    public string actionName = "TeleportUp"; // Name der Action für diesen Trigger

    private bool playerInRange;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        if (actionName == "TeleportUp")
        {
            playerControls.Player.TeleportUp.performed += OnTeleport;
        }
        else if (actionName == "TeleportDown")
        {
            playerControls.Player.TeleportDown.performed += OnTeleport;
        }
    }

    private void OnDisable()
    {
        if (actionName == "TeleportUp")
        {
            playerControls.Player.TeleportUp.performed -= OnTeleport;
        }
        else if (actionName == "TeleportDown")
        {
            playerControls.Player.TeleportDown.performed -= OnTeleport;
        }
        playerControls.Player.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnTeleport(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = teleportTarget.position;
        }
    }
}
