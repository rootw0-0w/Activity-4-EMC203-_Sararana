using UnityEngine;

public class Coin : MonoBehaviour
{
    public RectTransform uiTarget;
    public PlayerManager game;
    public float speed = 5f;

    void Update()
    {
        //moves to UI
        transform.position = Vector3.MoveTowards(transform.position, uiTarget.position, speed * Time.deltaTime);

        //adds number when coin has reached UI
        if (Vector3.Distance(transform.position, uiTarget.position) < 1f)
        {
            game.AddCoin();
            Destroy(gameObject);
        }
    }
}