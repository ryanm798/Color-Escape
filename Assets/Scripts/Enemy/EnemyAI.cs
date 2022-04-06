using System.Collections.Generic;
using UnityEngine;

// EnemyAI class obtained from Antarsoft's tutorial: https://www.youtube.com/watch?v=QxKcO0q7GR4
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    //Reference to waypoints
    public List<Transform> points;
    //The int value for next point index
    public int nextID = 0;
    //The value of that applies to ID for changing
    int idChangeValue = 1;
    //Speed of movement or flying
    public float speed = 2;
    public bool cyclePoints = false;

    [SerializeField] public bool lockOrientation = false;
    [SerializeField] public bool SetRotation = true;
    [SerializeField] public float rotateSpeed = 0;
    [SerializeField] private float waitTime = 1;
    private float remainingTime;
    private Animator animator;
    public GameObject turnWarning = null;
    public float warningAppear = 0.5f;

    public Vector3 fovOffest;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private Transform fovPrefab;
    private FieldOfView fieldOfView;


    private void Start()
    {
        remainingTime = waitTime;
        fieldOfView = Instantiate(fovPrefab, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.rotateSpeed = this.rotateSpeed;

        animator = GetComponent<Animator>();
        if (turnWarning != null)
            turnWarning.SetActive(false);
        warningAppear = Mathf.Clamp(warningAppear, 0, 1);
    }

    private void Update()
    {
        MoveToNextPoint();
        fieldOfView.SetOrigin(transform.position + fovOffest);
        if (SetRotation)
        {
            Vector3 dir = new Vector3(-transform.localScale.x, 0, 0);
            fieldOfView.SetAimDirection(dir);
            transform.RotateAround(transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

    void MoveToNextPoint()
    {
        if (points.Count == 0)
        {
            //if(rotate)
            return;
        }
        //Get the next Point transform
        Transform goalPoint = points[nextID];
        if (!lockOrientation)
        {
            //Flip the enemy transform to look into the point's direction
            if (goalPoint.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (goalPoint.transform.position.x < transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
        }
        //Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        //Check the distance between enemy and goal point to trigger next point
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.05f)
        {
            if (animator.GetBool("IsWalking"))
            {
                animator.SetBool("IsWalking", false);
            }
            
            if (points.Count == 1)
            {
                return;
            }
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                if ( (turnWarning != null) && ((1f - remainingTime / waitTime) >= warningAppear) )
                    turnWarning.SetActive(true);
                return;
            }

            //Check if we are at the end of the line (make the change -1)
            if (nextID == points.Count - 1)
                idChangeValue = -1;
            //Check if we are at the start of the line (make the change +1)
            if (nextID == 0 || cyclePoints)
                idChangeValue = 1;
            //Apply the change on the nextID
            nextID += idChangeValue;
            nextID = nextID % points.Count;
            remainingTime = waitTime;
        }
        else
        {
            if (!animator.GetBool("IsWalking"))
            {
                animator.SetBool("IsWalking", true);
                if (turnWarning != null)
                    turnWarning.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !PlayerState.Instance.IsHidden())
        {
            PlayerState.Instance.Respawn();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !PlayerState.Instance.IsHidden())
        {
            PlayerState.Instance.Respawn();
        }
    }
}