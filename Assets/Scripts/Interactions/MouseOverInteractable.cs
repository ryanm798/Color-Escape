using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseOverInteractable : MonoBehaviour
{
    [Header("Mouse Events")]
    public UnityEvent onMouseEnter;
    public UnityEvent onMouseHover;
    public UnityEvent onMouseExit;

    void Awake()
    {
        if (onMouseEnter == null)
			onMouseEnter = new UnityEvent();
        if (onMouseHover == null)
			onMouseHover = new UnityEvent();
        if (onMouseExit == null)
			onMouseExit = new UnityEvent();
    }

    void OnMouseEnter()
    {
        onMouseEnter.Invoke();
    }

    void OnMouseOver()
    {
        onMouseHover.Invoke();
    }

    void OnMouseExit()
    {
        onMouseExit.Invoke();
    }
}
