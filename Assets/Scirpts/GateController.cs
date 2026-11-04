using Enums;
using System;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private Collider2D gateCollider;
    [SerializeField] private Animator animator;

    private bool opened = false;

    private void OnEnable()
    {
        PowerUpsManager.PowerUpActive += OnPowerUpActive;
        PowerUpsManager.PowerUpEnd += OnPowerUpEnd;
    }
    private void OnDisable()
    {
        PowerUpsManager.PowerUpActive -= OnPowerUpActive;
        PowerUpsManager.PowerUpEnd -= OnPowerUpEnd;
    }



    private void OpenGate(bool open)
    {
        opened = open;
        gateCollider.enabled = !open;
        if (open)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paddle"))
        {
            GameManager.Instance.EnableNextLevel();
        }
    }

    private void OnPowerUpActive(EPowerUp powerUp)
    {
        if (powerUp == EPowerUp.Break)
        {
            if (!opened)
                OpenGate(true);
        }
    }

    private void OnPowerUpEnd(EPowerUp powerUp)
    {
        if(powerUp == EPowerUp.Break)
        {
            if (opened)
                OpenGate(false);
        }
    }
}