using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInteractable : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private MainMenuInteractable link = null;
    private Collider2D collider2d;
    
    public void SetLink(GameObject other)
    {
        MainMenuInteractable mmi = other.GetComponent<MainMenuInteractable>();
        if (mmi)
            SetLink(mmi);
    }
    public void SetLink(MainMenuInteractable other)
    {
        link = other;
    }

    void Start()
    {
        collider2d = GetComponent<Collider2D>();
    }

    void OnMouseEnter()
    {
        onMouseEnter();
        if (link != null)
        {
            link.onMouseEnter();
            link.DisableCollider();
        }
    }
    void onMouseEnter()
    {
        animator.SetBool("MouseOver", true);
    }

    void OnMouseExit()
    {
        onMouseExit();
        if (link != null)
        {
            link.onMouseExit();
            link.EnableCollider();
        }
    }
    void onMouseExit()
    {
        animator.SetBool("MouseOver", false);
    }

    private void EnableCollider()
    {
        collider2d.enabled = true;
    }

    private void DisableCollider()
    {
        collider2d.enabled = false;
    }
}
