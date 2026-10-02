using UnityEngine;

public class CubicSpawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public Transform controlA;
    public Transform controlB;
    public Transform target;
    public float timeToReachTarget = 5f;
    public float spawnInterval = 5f;
    public float resolution = 20f;

    private GameObject creature;
    private Vector3 initialPosition;
    private float totalTime;
    private float spawnTimer;

    void Start()
    {
        initialPosition = transform.position;
        Spawn();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            Spawn();
        }

        if (creature == null) return;

        totalTime += Time.deltaTime;
        float t = Mathf.Clamp01(totalTime / timeToReachTarget);

        creature.transform.position = CubicFast(initialPosition, controlA.position, controlB.position, target.position, t);

        if (t >= 1f)
        {
        Destroy(creature);
        creature = null;
        }
    }

    void Spawn()
    {
        if (creature != null) Destroy(creature);
        initialPosition = transform.position;
        creature = Instantiate(creaturePrefab, initialPosition, Quaternion.identity);
        totalTime = 0f;
    }

    public static Vector3 CubicFast(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return u*u*u*p0 + 3*u*u*t*p1 + 3*u*t*t*p2 + t*t*t*p3;
    }

    private void OnDrawGizmos()
    {
        if (controlA == null || controlB == null || target == null) return;
        Vector3 previousLine = initialPosition;
        for (int i = 1; i <= resolution; i++)
        {
            float gap = i / resolution;
            Vector3 newPos = CubicFast(initialPosition, controlA.position, controlB.position, target.position, gap);
            Debug.DrawLine(previousLine, newPos, Color.red);
            previousLine = newPos;
        }
    }
}