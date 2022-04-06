using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    float viewDistance;
    private Vector3 origin;
    public float startingAngle;
    public float rotateSpeed;
    [SerializeField] public float rotate = 0;

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) {
            n += 360;
        }

        return n;
    }


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 45f;
        viewDistance = 4f;
        origin = Vector3.zero;
    }

    private void Update() {
        //fields
        rotate += rotateSpeed * Time.deltaTime;
        int rayCount = 50;
        float angle = startingAngle + rotate;
        float angleIncrease = fov / rayCount;

        //setup UVs, vertices, and triangles for the mesh
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        //cycle through arrays
        for (int i = 0; i <= rayCount; i++)
        {
            //locate on correct positions
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, layerMask);
            if(raycastHit2D.collider == null)
            {
                //no hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //hit object
                if(raycastHit2D.collider.tag == "Player")
                {
                    //Debug.Log("Found");
                    PlayerState.Instance.Respawn();
                }
                vertex = raycastHit2D.point - (Vector2)transform.position;
            }
            vertices[vertexIndex] = vertex;

            //generate triangles
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex += 1;
            //move on to next angle
            angle -= angleIncrease;
        }

        //upload to mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    public void SetOrigin(Vector3 origin)
    {
        transform.position = origin;
        this.origin = new Vector3(0,0,0);
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float distance)
    {
        viewDistance = distance;
    }
}
