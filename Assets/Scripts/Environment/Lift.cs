using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float speed = 20f;
    public float waitTime = 1f;
    private float remainingTime;
    public List<Transform> points;
    private int nextPoint = 0;
    private int nextPointDir = 1;

    private Rigidbody2D player;
    private Vector3 moveDelta;

    void Start()
    {
        remainingTime = waitTime;
    }

    void Update()
    {
        SetMoveDirection();
        SetMovement();
    }

    //Move the player by the change in movement of the platform using the player's rigidbody instead of parenting
    void LateUpdate()
    {
        if (player) 
        {
            Vector2 playerBody = player.position;
            player.transform.position = new Vector3(playerBody.x, playerBody.y, player.transform.position.z) + moveDelta;
        }
    }

    void SetMovement()
    {
        Vector2 desiredPosition = Vector2.MoveTowards(transform.position, points[nextPoint].position, speed * Time.deltaTime);

        //Use that position to figure out the change in position of the platform
        moveDelta = new Vector3(desiredPosition.x, desiredPosition.y, 0f) - transform.position;
        moveDelta.z = 0f;

        //Apply the new position
        transform.position = desiredPosition;
    }

    void SetMoveDirection()
    {
        if (Vector2.Distance(transform.position, points[nextPoint].position) <= 0.1)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                return;
            }

            if (nextPoint == points.Count - 1)
                nextPointDir = -1;
            else if (nextPoint == 0)
                nextPointDir = 1;
            nextPoint += nextPointDir;
            remainingTime = waitTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            player = other.gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            player = null;
    }
 }