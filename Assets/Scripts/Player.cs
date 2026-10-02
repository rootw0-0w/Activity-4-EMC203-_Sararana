using UnityEngine;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour 
{ 
    [SerializeField] private float speed = 10f; 
    
    public float finish_zone = 2f; 
    public Transform PlayerObject; 
    public GameObject winPanel; 

   
    [SerializeField] private float hitDistance = 0.8f; // bullet distance

    void Start() 
    { 
        if (winPanel != null) 
        { 
            winPanel.SetActive(false); 
        } 
    } 

    void Update() 
    { 
        Move(); 
        WinPanel(); 
        DetectShoot(); //looks for bullets for reset
    } 

    void Move() 
    { 
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical"); 
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput); 
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); 
    } 

    void WinPanel() 
    { 
        if (PlayerObject == null) return; 

        float distance = Vector3.Distance(transform.position, PlayerObject.position); 

        if (distance <= finish_zone) 
        { 
            if (winPanel != null) 
            { 
                winPanel.SetActive(true); 
            } 
        } 
    } 

    void DetectShoot()
    {
        // 1. Find every active rocket in the scene using their Tag
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        
        foreach (GameObject rocket in bullets)
        {
            if (rocket != null)
            {
                //using vectors calculates distance between obj
                float distance = Vector3.Distance(transform.position, rocket.transform.position);

                //any close bullets resets the game
                if (distance <= hitDistance)
                {
                    RestartScene();
                    return; // Stops checking
                }
            }
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
