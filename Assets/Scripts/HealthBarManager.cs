using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] Image ghostBar;
    public float changespeed = 10f;
    public float ghostSpeed = .5f;
    [SerializeField] private float delayTime = 0.4f;

    public float fillAmount { get; set; } = 1.0f;

    private float totalTime;
    private float lastFillAmount = 1.0f;

    public static float EaseOutCubic(float t) => 1 - (1 - t) * (1 - t) * (1 - t);

    void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, fillAmount, changespeed * Time.deltaTime);

        if (fillAmount != lastFillAmount) //checks hp bar changes at every health hit
        {
            totalTime = 0f;
            lastFillAmount = fillAmount;
        }

        totalTime += Time.deltaTime;

        if (totalTime >= delayTime)
        {
            float t = Mathf.Clamp01(ghostSpeed * Time.deltaTime);
            ghostBar.fillAmount = Mathf.Lerp(ghostBar.fillAmount, healthBar.fillAmount, EaseOutCubic(t));
        }
    }
}