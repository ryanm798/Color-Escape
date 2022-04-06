using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Placement : Interactable
{
    [Header("Placement")]
    public string Target;
    public Transform PlacementPosition;
    public float PlacedScaleFactor = 1f;
    public bool AllowPickBackUp = true;
    private Pickup Item = null;
    public UnityEvent OnSuccessfulPlacement;
    public UnityEvent OnSuccessfulPickup;


    override protected void Awake()
    {
        base.Awake();

        if (OnSuccessfulPlacement == null)
			OnSuccessfulPlacement = new UnityEvent();
        if (OnSuccessfulPickup == null)
            OnSuccessfulPickup = new UnityEvent();
    }

    override public void Interact()
    {
        base.Interact();

        if (Item == null)
        {
            Item = Inventory.Instance.Remove(Target);
            if (Item != null)
            {
                OnSuccessfulPlacement.Invoke();

                Item.enabled = false;
                if (PlacementPosition != null)
                {
                    Item.gameObject.SetActive(true);
                    Item.gameObject.transform.position = PlacementPosition.position;
                    Item.gameObject.transform.localScale *= PlacedScaleFactor;
                }

                if (!AllowPickBackUp)
                {
                    this.enabled = false;
                }
            }
        }
        else if (AllowPickBackUp)
        {
            OnSuccessfulPickup.Invoke();
            
            Item.Interact();
            Item = null;
        }
    }

    public void SmartIndicatorUpdate()
    {
        Indicator.SetActive( (Item != null && AllowPickBackUp) || (Item == null && Inventory.Instance.Contains(Target)) );
    }
}
