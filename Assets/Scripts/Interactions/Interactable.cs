using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interactable")]
    public Collider2D Trigger;
    public bool Required = false;
    public bool Automatic = true;

    [Header("Indicator")]
    public GameObject Indicator;
    public enum IndicatorShow
    {
        Never,
        WhenInRange,
        WhenInRangeAndStay,
        Always
    }
    [SerializeField]
    private IndicatorShow ShowIndicator = IndicatorShow.WhenInRange;
    public bool HideIndicatorOnDisable = true;

    [Header("Events")]
    public UnityEvent OnInteract;

    
    virtual protected void Awake()
    {
        if (OnInteract == null)
			OnInteract = new UnityEvent();
    }

    virtual protected void Start()
    {
        Trigger.isTrigger = true;
        if (Automatic)
        {
            ShowIndicator = IndicatorShow.Never;
            if (Indicator != null)
                Indicator.SetActive(false);
        }
    }
    
    virtual protected void Update()
    {
        
    }

    void OnEnable()
    {
        Trigger.enabled = true;
        if (Indicator != null)
        {
            if (ShowIndicator == IndicatorShow.Always)
                Indicator.SetActive(true);
            else
                Indicator.SetActive(false);
        }
    }

    void OnDisable()
    {
        MakeUnavailable();
        Trigger.enabled = false;
        if (Indicator != null && HideIndicatorOnDisable)
            Indicator.SetActive(false);
    }

    virtual public void Interact()
    {
        if (Required)
        {
            Required = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnableMovement();
        }
        OnInteract.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Player")
        {
            if (Automatic)
            {
                Interact();
            }
            else
            {
                if (Required)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().DisableMovement();
                }

                if (Indicator != null && ShowIndicator != IndicatorShow.Never)
                {
                    Indicator.SetActive(true);
                }
                MakeAvailable();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Player")
        {
            if (!Automatic)
            {
                if (Required)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnableMovement();
                }

                if (Indicator != null && ShowIndicator == IndicatorShow.WhenInRange)
                {
                    Indicator.SetActive(false);
                }
                MakeUnavailable();
            }
        }
    }

    protected void MakeAvailable()
    {
        PlayerState.Instance.AvailableInteraction = this;
    }

    protected void MakeUnavailable()
    {
        if (PlayerState.Instance != null && PlayerState.Instance.AvailableInteraction == this)
            PlayerState.Instance.AvailableInteraction = null;
    }
}
