using UnityEngine;

public class Turret : MonoBehaviour
{
    public float speed = 10f;
    public float hitDistance = 0.5f;
    public PlayerManager game;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject c in creatures)
        {
            if (c == null) continue;

            //destroys on impact
            if (Vector3.Distance(transform.position, c.transform.position) <= hitDistance)
            {
                game.OnCreatureKilled(c.transform.position);
                Destroy(c);
                return;
            }
        }
    }
}