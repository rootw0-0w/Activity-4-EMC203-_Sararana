using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public float health = 20f;
    public float maxHealth = 20f;
    public HealthBarManager healthBar;
    public float hitDistance = 0.5f;

    [Header("Bank")]
    public int coins;
    public TextMeshProUGUI coinText;
    public RectTransform punchTarget;
    public float punchScale = 1.3f;
    public float punchDuration = 0.4f;
    public float countSpeed = 5f;

    [Header("Coin")]
    public GameObject coinPrefab;
    public RectTransform coinUITarget;

    private Vector3 baseScale;
    private float displayed;
    private float punchTime = 1f;

    void Start()
    {
        baseScale = punchTarget.localScale;
        displayed = coins;
        coinText.text = coins.ToString();
    }

    void Update()
    {
        // health bar
        healthBar.fillAmount = health / maxHealth;

        // player takes damage from creatures near them
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject c in creatures)
        {
            if (c == null) continue;

            if (Vector3.Distance(transform.position, c.transform.position) <= hitDistance)
            {
                health = Mathf.Max(health - 1f, 0f);
                Destroy(c);
            }
        }

        // coin number easing toward real value
        float t = Mathf.Clamp01(countSpeed * Time.deltaTime);
        displayed = Mathf.Lerp(displayed, coins, EaseOutCubic(t));
        coinText.text = Mathf.RoundToInt(displayed).ToString();


        // punch bounce animation
        if (punchTime < 1f)
        {
            punchTime += Time.deltaTime / punchDuration;
            float mult = Mathf.Lerp(punchScale, 1f, EaseOutBounce(Mathf.Clamp01(punchTime)));
            punchTarget.localScale = baseScale * mult;
        }
    }

    public void OnCreatureKilled(Vector3 pos)
    {
        SpawnCoin(pos);
    }

    public void AddCoin()
    {
        coins++;
        punchTime = 0f;
    }

    void SpawnCoin(Vector3 pos) //takes the prefab directly for the UI score input
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
        Coin c = coin.GetComponent<Coin>();
        c.uiTarget = coinUITarget;
        c.game = this;
    }

    public static float EaseOutCubic(float t) => 1 - (1 - t) * (1 - t) * (1 - t);

    public static float EaseOutBounce(float t)
    {
        float n1 = 7.5625f, d1 = 2.75f;
        if (t < 1f / d1) return n1 * t * t;
        if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
        if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
        t -= 2.625f / d1; return n1 * t * t + 0.984375f;
    }
}