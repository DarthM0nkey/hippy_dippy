using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[ExecuteInEditMode]
public class WaterBody : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField]
    float width = 10f;
    [SerializeField]
    float height = 3f;
    [SerializeField]
    int quality = 1;

    [Header("Physics Settings")]
    public float springconstant = 0.02f;
    public float damping = 0.04f;
    public float spread = 0.05f;
    public float collisionVelocityFactor = 0.04f;

    private Vector3[] vertices;
    private float[] velocities;
    private float[] accelerations;
    private float[] leftDeltas;
    private float[] rightDeltas;
    private float timer;
    int lastHit = -1;

    [SerializeField]
    BoxCollider2D boxCollider2D;
    [SerializeField]
    BoxCollider2D underwaterCollider2D;

    [Header("Display Settings")]
    [SerializeField]
    MeshFilter meshFilter;

    
    private void OnValidate()
    {
        Mesh mesh = GenerateMesh();
        meshFilter.mesh = mesh;

        boxCollider2D.offset = new Vector2(width / 2f, -height / 2f);
        boxCollider2D.size = new Vector2(width, height);
        boxCollider2D.isTrigger = true;

        boxCollider2D.offset = new Vector2(width / 2f, -height / 2f);
        boxCollider2D.size = new Vector2(width, height);
        underwaterCollider2D.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        Mesh mesh = meshFilter.sharedMesh;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireMesh(mesh, transform.position);
        Gizmos.color = Color.white;
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            if (lastHit == i)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + verts[i], 0.1f);
        }

        
    }

    private Mesh GenerateMesh()
    {
        float interval = width / (quality - 1);
        vertices = new Vector3[quality * 2];
        // generate vertices
        // top vertices
        for (int i = 0; i < quality; i++)
        {
            vertices[i] = new Vector3(i * interval, 0, 0);
        }

        // bottom vertices
        for (int i = 0; i < quality; i++)
        {
            vertices[i + quality] = new Vector3(i * interval, -height, 0);
        }
        // generate tris. the algorithm is messed up but works. lol.
        int[] template = new int[6];
        template[0] = quality;
        template[1] = 0;
        template[2] = quality + 1;
        template[3] = 0;
        template[4] = 1;
        template[5] = quality + 1;
        int marker = 0;
        int[] tris = new int[(quality - 1) * 2 * 3];
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = template[marker++]++;
            if (marker >= 6) marker = 0;
        }
        // generate mesh
        var the_mesh = new Mesh
        {
            vertices = vertices,
            triangles = tris
        };
        the_mesh.RecalculateNormals();
        the_mesh.RecalculateBounds();
        // set up mesh
        return the_mesh;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        Splash(col, rb.velocity.y * collisionVelocityFactor);
    }

    public void Splash(Collider2D col, float force)
    {
        timer = 3f;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, col.bounds.center.y);
        // instantiate splash particle
        //GameObject splashGO = Instantiate(splash, new Vector3(center.x, center.y, 0), Quaternion.Euler(0, 0, 60));
        //Destroy(splashGO, 2f);
        // applying physics
        for (int i = 0; i < quality; i++)
        {
            if (PointInsideCircle(transform.position + vertices[i], center, radius))
            {
                lastHit = i;
                velocities[i] = force;
            }
        }
    }

    bool PointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }

    // Start is called before the first frame update
    void Start()
    {
        velocities = new float[quality];
        accelerations = new float[quality];
        leftDeltas = new float[quality];
        rightDeltas = new float[quality];

        Mesh mesh = GenerateMesh();
        meshFilter.mesh = mesh;

        boxCollider2D.offset = new Vector2(width / 2f, -height / 2f);
        boxCollider2D.size = new Vector2(width, height);
        boxCollider2D.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = meshFilter.sharedMesh;
        // optimization. we don't want to calculate all of this on every update.
        if (timer <= 0) 
            return;

        timer -= Time.deltaTime;
        // updating physics
        for (int i = 0; i < quality; i++)
        {
            float force = springconstant * (vertices[i].y - transform.position.y) + velocities[i] * damping;
            accelerations[i] = -force;
            vertices[i].y += velocities[i];
            velocities[i] += accelerations[i];
        }

        for (int i = 0; i < quality; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = spread * (vertices[i].y - vertices[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }
            if (i < quality - 1)
            {
                rightDeltas[i] = spread * (vertices[i].y - vertices[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }
        // updating mesh
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
