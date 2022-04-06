using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{
    [Header("Pickup")]
    [Tooltip("Must be unique.")]
    public string Name;
    public GameObject InventoryPrefab;

    [Header("Animation")]
    public bool Animate = true;
    public float Amplitude = 0.003f;
    public float Frequency = 0.8f;

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();

        if (Animate)
        {
            Vector3 tempPos = transform.position;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude;
            transform.position = tempPos;
        }
    }

    override public void Interact()
    {
        base.Interact();

        gameObject.SetActive(false);
        
        Inventory.Instance.Add(this);
    }
}
