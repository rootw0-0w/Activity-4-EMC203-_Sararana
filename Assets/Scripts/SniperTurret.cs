using UnityEngine;

public class SniperTurret : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotSpeed = 10f; 
    [SerializeField] private GameObject bulletPrefab;
    
    private bool hasFired = false; //using bool to ensure single shot

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - this.transform.position;
        float targetAngle = Mathf.Atan2(direction.x, direction.z); //rotate in a flat surface
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotSpeed * Time.deltaTime); //spins to detect player

        Vector3 barrel = this.transform.forward.normalized; 
        Vector3 toPlayer = direction.normalized;

        
        float dot = Vector3.Dot(barrel, toPlayer);
        bool inSights = dot >= 0.98f; 

        if (inSights)
        {
            if (!hasFired)
            {
                ShootProjectile(); 
                hasFired = true; // ensures single fire
            }
        }
        else
        {
            //fire when player resets to range
            hasFired = false;
        }
    }

    void ShootProjectile()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
            
             Destroy(bullet, 3f);
        }
    }
}
