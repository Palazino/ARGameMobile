using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageOverlay; // Image rouge pour clignoter lors des dégâts
    public float shakeDuration = 0.5f; // Durée du tremblement
    public float shakeMagnitude = 0.2f; // Intensité du tremblement
    public float flashDuration = 0.5f; // Durée du flash rouge

    private Camera mainCamera;
    private Vector3 initialCameraPosition;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        mainCamera = Camera.main;
        initialCameraPosition = mainCamera.transform.position;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthSlider();
        StartCoroutine(ScreenShake());
        StartCoroutine(FlashRed());
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthSlider();
    }

    void UpdateHealthSlider()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }

    IEnumerator ScreenShake()
    {
        float elapsedTime = 0;
        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = initialCameraPosition + Random.insideUnitSphere * shakeMagnitude;
            mainCamera.transform.position = randomPoint;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = initialCameraPosition;
    }

    IEnumerator FlashRed()
    {
        float elapsedTime = 0;
        while (elapsedTime < flashDuration)
        {
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, Mathf.Lerp(0, 1, elapsedTime / flashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < flashDuration)
        {
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, Mathf.Lerp(1, 0, elapsedTime / flashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
    }
}
