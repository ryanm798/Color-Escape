using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal = true;
    [SerializeField] private bool infiniteVertical = true;
    [SerializeField] private bool syncYPos = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;


    void Start()
    {
        if (syncYPos)
        {
            infiniteVertical = false;
            parallaxEffectMultiplier.y = 1;
        }

        Vector3 startPos = Camera.main.transform.position;
        startPos.z = transform.position.z;
        transform.position = startPos;

        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, deltaMovement.z);
        lastCameraPosition = cameraTransform.position;

        if (infiniteHorizontal && (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX))
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
        if (infiniteVertical && (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY))
        {
            float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y + offsetPositionY, transform.position.z);
        }
    }
}
