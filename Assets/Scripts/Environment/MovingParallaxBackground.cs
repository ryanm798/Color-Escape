using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParallaxBackground : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool direction = true;
    [SerializeField] private bool repeatChildren = true;

    private float directionMultiplier;
    private float cameraPositionX;
    private float textureUnitSizeX;


    void Start()
    {
        cameraPositionX = Camera.main.transform.position.x;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        if (direction) directionMultiplier = 1;
        else directionMultiplier = -1;

        if (repeatChildren)
        {
            // MUST get array of children in first loop and instantiate copies in second separate loop;
            // a single loop will be infinite as copies become children to be looped through
            Transform[] children = new Transform[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                children[i++] = child;
            }

            foreach (Transform child in children)
            {
                GameObject copy = Instantiate(child.gameObject, transform);
                copy.transform.position = new Vector3(child.position.x - directionMultiplier * textureUnitSizeX, child.position.y, child.position.z);
                
                MainMenuInteractable mmi = child.GetComponent<MainMenuInteractable>();
                if (mmi)
                {
                    mmi.SetLink(copy);
                    copy.GetComponent<MainMenuInteractable>().SetLink(child.gameObject);
                }
            }
        }
    }

    void Update()
    {
        float deltaX = directionMultiplier * Time.deltaTime * speed;
        transform.position += new Vector3(deltaX, 0, 0);

        if (Mathf.Abs(transform.position.x - cameraPositionX) >= textureUnitSizeX)
        {
            float offsetPositionX = (transform.position.x - cameraPositionX) % textureUnitSizeX;
            transform.position = new Vector3(cameraPositionX + offsetPositionX, transform.position.y, transform.position.z);
        }
    }
}
