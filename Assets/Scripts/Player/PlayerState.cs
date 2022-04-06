using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerState : MonoBehaviour
{
    /***** SINGLETON SETUP *****/
    private static PlayerState _instance;
    public static PlayerState Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }
    }
    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
    /******************************/


    [System.NonSerialized]
    public Interactable AvailableInteraction = null;

    private bool hidden = false;
    private bool onFallthrough = false;

    [Header("Color Glow")]
    [Tooltip("The max distance between the player and an orb within which the player will glow. We're assuming only 1 orb will be within this distance at a time.")]
    public float MaxDistance = 10.0f;
    public float MaxLightIntensity = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Color defaultSpriteColor;
    private Light2D playerLight;
    private Color defaultLightColor;
    private float defaultLightIntensity;

    [Header("Respawn")]
    public Transform RespawnPoint;
    public Animator RespawnAnimator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSpriteColor = spriteRenderer.color;
        playerLight = GetComponentInChildren<Light2D>();
        defaultLightColor = playerLight.color;
        defaultLightIntensity = playerLight.intensity;
        MaxLightIntensity = Mathf.Clamp(MaxLightIntensity, 0f, 1f);
        if (MaxLightIntensity < defaultLightIntensity)
            MaxLightIntensity = defaultLightIntensity;
    }

    void Update()
    {
        if (Input.GetKeyDown("f") && AvailableInteraction != null)
        {
            AvailableInteraction.Interact();
        }
    }

    public void UpdateColor(float distance, Color targetColor)
    {
        float t = Mathf.Clamp(1.0f - distance / MaxDistance, 0f, 1f);
        spriteRenderer.color = Color.Lerp(defaultSpriteColor, targetColor, t);
        playerLight.color = Color.Lerp(defaultLightColor, targetColor, t);
        playerLight.intensity = t*MaxLightIntensity + (1f - t)*defaultLightIntensity;
    }

    public void ResetColor()
    {
        spriteRenderer.color = defaultSpriteColor;
        playerLight.color = defaultLightColor;
        playerLight.intensity = defaultLightIntensity;
    }

    public void Respawn()
    {
        if (RespawnPoint != null)
        {
            if (RespawnAnimator != null)
            {
                RespawnAnimator.SetTrigger("Respawn");
            }
            else
            {
                InstantRespawn();
            }
        }
    }
    public void InstantRespawn()
    {
        transform.position = RespawnPoint.position;
        PlayerState.Instance.ResetColor();
    }

    public void ChangeRespawnPoint(Transform newPoint)
    {
        RespawnPoint = newPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bush")
        {
            hidden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bush")
        {
            hidden = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Fallthrough")
        {
            onFallthrough = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Fallthrough")
        {
            onFallthrough = false;
        }
    }

    public bool IsHidden()
    {
        return hidden;
    }
}
