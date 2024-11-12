using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageOverlay; // Image rouge pour clignoter lors des d�g�ts
    public Image blurImage; // Image pour l'effet de brouillage
    public TextMeshProUGUI gameOverText; // Texte Game Over
    public float shakeDuration = 0.5f; // Dur�e du tremblement
    public float shakeMagnitude = 0.2f; // Intensit� du tremblement
    public float flashDuration = 0.5f; // Dur�e du flash rouge
    public float blurDuration = 2.0f; // Dur�e de l'effet de brouillage

    private Camera mainCamera;
    private Vector3 initialCameraPosition;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        mainCamera = Camera.main;
        initialCameraPosition = mainCamera.transform.position;
        blurImage.gameObject.SetActive(false); // D�sactiver l'image de brouillage au d�part
        gameOverText.gameObject.SetActive(false); // D�sactiver le texte Game Over au d�part
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

        if (currentHealth == 0)
        {
            StartCoroutine(GameOverSequence());
        }
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

    IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f; // Mettre le jeu en pause

        blurImage.gameObject.SetActive(true);

        float elapsedTime = 0;
        while (elapsedTime < blurDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / blurDuration);
            blurImage.color = new Color(blurImage.color.r, blurImage.color.g, blurImage.color.b, alpha);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        blurImage.color = new Color(blurImage.color.r, blurImage.color.g, blurImage.color.b, 1);
        gameOverText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2.0f); // Attendre 2 secondes
        SceneManager.LoadScene("MainMenu"); // Retourner au menu principal ou red�marrer le jeu
    }
}
