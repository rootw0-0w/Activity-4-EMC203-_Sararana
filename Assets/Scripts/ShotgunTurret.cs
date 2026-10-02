using UnityEngine;

public class ShotgunTurret : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private float detectionConeAngle = 45f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerManager game;

    private bool hasFired = false;

    void Start()
    {
    }

    void Update()
    {
        
        target = FindCreature();//forces the turrets to look for the creature
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
    


    void FireShotgunSpread() 
    {
        float bulletSpacing = 20f; 
        for (int slot = -1; slot <= 1; slot++) // keep looping until all bullets have been properly slotted in place
        {
            float angleOffset = (slot * bulletSpacing) + transform.eulerAngles.y; //determines the placement of the bullets if right center left
            Quaternion bulletRotation = Quaternion.Euler(0, angleOffset, 0);
            
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position, bulletRotation);
            bullet.GetComponent<Turret>().game = game; //contains player manager script to pass on bullet
            Destroy(bullet, 3f);
        }    
    }
}
