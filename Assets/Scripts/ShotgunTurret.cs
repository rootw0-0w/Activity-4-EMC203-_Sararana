using UnityEngine;

public class ShotgunTurret : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private float detectionConeAngle = 45f;
    [SerializeField] private GameObject bulletPrefab;

    private LineRenderer lineRenderer;
    private bool hasFired = false;

    void Start()
    {
        // renderer is automatically setup when played
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

        if (IsInCone(direction, fireRange, detectionConeAngle))
        {
             if (!hasFired)
            {
                FireShotgunSpread();
                hasFired = true;
            }
        }
        else
        {
            //repeated enter to range triggers shoot
            hasFired = false;
        }
        
    }

    bool IsInCone(Vector3 direction, float range, float coneAngle) //ConeDetection 
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
        //Creates a line based on turret detection shape 
        Vector3 leftDir = Quaternion.Euler(0, transform.eulerAngles.y - (detectionConeAngle / 2f), 0) * Vector3.forward;
        Vector3 rightDir = Quaternion.Euler(0, transform.eulerAngles.y + (detectionConeAngle / 2f), 0) * Vector3.forward;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + (leftDir * fireRange));
        lineRenderer.SetPosition(2, origin + (rightDir * fireRange));
    }

    void FireShotgunSpread() 
    {
        float bulletSpacing = 20f; 
        for (int slot = -1; slot <= 1; slot++) // keep looping until all bullets have been properly slotted in place
        {
            float angleOffset = (slot * bulletSpacing) + transform.eulerAngles.y; //determines the placement of the bullets if right center left
            Quaternion bulletRotation = Quaternion.Euler(0, angleOffset, 0);
            
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position, bulletRotation);
            Destroy(bullet, 3f);
        }    
    }
}
