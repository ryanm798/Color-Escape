using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Pickup
{
    [Header("Orb")]
    public Color PlayerReactColor;
    private Transform playerTransform;

    override protected void Start()
    {
        base.Start();

        playerTransform = PlayerState.Instance.gameObject.transform;
    }

    private float CalculateDistanceToPlayer()
    {
        return Mathf.Sqrt(Mathf.Pow(transform.position.x - playerTransform.position.x, 2) + Mathf.Pow(transform.position.y - playerTransform.position.y, 2));
    }

    override protected void Update()
    {
        base.Update();
        
        float dist = CalculateDistanceToPlayer();
        if (dist <= PlayerState.Instance.MaxDistance)
        {
            PlayerState.Instance.UpdateColor(dist, PlayerReactColor);
        }
    }

    public override void Interact()
    {
        base.Interact();

        PlayerState.Instance.ResetColor();
    }
}
