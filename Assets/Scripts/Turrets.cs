using UnityEngine;


public class Turrets : MonoBehaviour
{
    private float speed = 5f;
    

    void Update()
    {
        // Moves Rocket forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

   
}
