using UnityEngine;

public class FlameTurret : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private float detectionConeAngle = 45f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerManager game;


    void Start()
    {
    
    }

    void Update()
    {
        target = FindCreature(); //forces the turrets to look for the creature
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
    
     Transform FindCreature()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature"); 
        foreach (GameObject c in creatures)//keeps detecting if there are more than one creature
        {
            if (c == null) continue; 
            return c.transform;
        }
        return null;
    }

    void FireContinuousStream() 
    {
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Turret>().game = game; //contains player manager script to pass on bullet
        Destroy(bullet, 2f);
    }
}

