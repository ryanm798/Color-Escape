using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpatialTrigger : MonoBehaviour
{
    public Collider2D Trigger;
    
    [Header("Events")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    void Awake()
    {
        if (onTriggerEnter == null)
			onTriggerEnter = new UnityEvent();
    }

    void Start()
    {
        Trigger.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Player")
        {
            onTriggerEnter.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Player")
        {
            onTriggerExit.Invoke();
        }
    }
}
