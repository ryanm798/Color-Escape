using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelChanger : MonoBehaviour
{
    /***** SINGLETON SETUP *****/
    private static LevelChanger _instance;
    public static LevelChanger Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;

            if (OnSceneEnd == null)
			    OnSceneEnd = new UnityEvent();
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


    public bool MainMenu = false;

    [Header("Fade Animations")]
    public Animator animator;
    public float FadeOutTimeScale = 1;
    public bool FadeOut = true;
    
    [Header("Events")]
    public UnityEvent OnSceneEnd;

    [Header("Levels")]
    public int NumOrbs = 4;
    private int orbsReturned = 0;
    public Light2D GoalLight;
    public float MaxLightIntensity = 1f;

    private PlayerMovement playerMovement = null;
    
    private int levelToLoad = 0;


    void Start()
    {
        levelToLoad = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if (MainMenu)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;

            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            UpdateLight();
        }
    }

    public void OrbReturned()
    {
        UpdateLight();
        if (++orbsReturned >= NumOrbs)
        {
            LoadNextLevel();
        }
    }

    private void UpdateLight()
    {
        GoalLight.intensity = Mathf.Clamp(((float) orbsReturned) / NumOrbs, 0f, 1f) * MaxLightIntensity;
    }
    
    public void LoadNextLevel()
    {
        LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void LoadScene(int buildIndex)
    {
        OnSceneEnd.Invoke();

        GameManager.Instance.CurrentLevel = buildIndex;
        if (buildIndex == 0)
            GameManager.Instance.CurrentLevel = 1;

        levelToLoad = buildIndex;
        if (FadeOut)
        {
            animator.SetTrigger("FadeOut");
        }
        else
        {
            SceneManager.LoadScene(buildIndex);
        }
    }

    public void OnFadeInStart()
    {
        DisablePlayerMovement();
    }

    public void OnFadeInEnd()
    {
        EnablePlayerMovement();
    }

    public void OnFadeOutStart()
    {
        //DisablePlayerMovement();
        GameManager.Instance.ScaleTime(FadeOutTimeScale);
    }

    public void OnFadeOutEnd()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void EnablePlayerMovement()
    {
        if (playerMovement != null)
            playerMovement.EnableMovement();
    }

    private void DisablePlayerMovement()
    {
        if (playerMovement != null)
            playerMovement.DisableMovement();
    }
}
