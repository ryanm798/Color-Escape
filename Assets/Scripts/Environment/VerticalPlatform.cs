using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();    
    }

    void Update()
    {
        if (Input.GetButton("Crouch"))
        {
            effector.rotationalOffset = 180f;
        } else {
            effector.rotationalOffset = 0f;
        }
    }
}
