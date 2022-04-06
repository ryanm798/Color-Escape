using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAnimation : MonoBehaviour
{
    private Animator respawnAnimator;
    private PlayerMovement playerMovement;
    public float respawnTimeScale = 1;

    private void Start()
    {
        respawnAnimator = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void OnFadeOutStart()
    {
        GameManager.Instance.ScaleTime(respawnTimeScale);
    }

    public void OnFadeInStart()
    {
        GameManager.Instance.ScaleTime(1);
        PlayerState.Instance.InstantRespawn();
        if (playerMovement != null)
            playerMovement.DisableMovement();
        respawnAnimator.ResetTrigger("Respawn");
    }

    public void OnFadeInEnd()
    {
        if (playerMovement != null)
            playerMovement.EnableMovement();
    }
}
