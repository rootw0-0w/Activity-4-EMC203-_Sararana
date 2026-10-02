using UnityEngine;

public class FlameTurret : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private float detectionConeAngle = 45f;
    [SerializeField] private GameObject bulletPrefab;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Line renderer sets up automatically on startup
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        DrawSimpleCone();

        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        // repeated check continous fire if player is in the range stops when gone
        if (IsInCone(direction, fireRange, detectionConeAngle))
        {
            FireContinuousStream();
        }
    }

    bool IsInCone(Vector3 direction, float range, float coneAngle)
    {
        if (direction.magnitude > range)
            return false;

        float pAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float tAngle = transform.eulerAngles.y;

        float delta = Mathf.Abs(Mathf.DeltaAngle(tAngle, pAngle));
        return delta <= coneAngle / 2f;
    }

    void DrawSimpleCone()
    {
        Vector3 origin = transform.position;

        Vector3 leftDir = Quaternion.Euler(0, transform.eulerAngles.y - (detectionConeAngle / 2f), 0) * Vector3.forward;
        Vector3 rightDir = Quaternion.Euler(0, transform.eulerAngles.y + (detectionConeAngle / 2f), 0) * Vector3.forward;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + (leftDir * fireRange));
        lineRenderer.SetPosition(2, origin + (rightDir * fireRange));
    }

    void FireContinuousStream() 
    {
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        Destroy(bullet, 2f);
    }
}

