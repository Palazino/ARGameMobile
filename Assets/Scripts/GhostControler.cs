using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour
{
    public int health = 100; // Points de vie du fant�me
    public int xpValue = 50; // Valeur en XP du fant�me
    private float maxRotationSpeed = 360f; // Vitesse de rotation initiale
    private bool isRotating = false;
    private AudioSource audioSource;
    private Vector3 initialScale;
    private bool isTakingDamage = false;
    private float moveSpeed = 0.5f; // Vitesse de d�placement vers la cam�ra
    private float shrinkDuration = 2.0f; // Dur�e du r�tr�cissement en secondes

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale;
        FaceCamera(); // Tourner vers la cam�ra lors de l'apparition
    }

    void Update()
    {
        // Avancer vers la cam�ra
        MoveTowardsCamera();

        if (isRotating)
        {
            // Appliquer la rotation
            transform.Rotate(Vector3.up, maxRotationSpeed * Time.deltaTime);
        }
    }

    void MoveTowardsCamera()
    {
        if (!isTakingDamage && !isRotating) // Avancer seulement quand il ne prend pas de coups et ne dispara�t pas
        {
            Vector3 directionToCamera = (Camera.main.transform.position - transform.position).normalized;
            transform.position += directionToCamera * moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isTakingDamage)
        {
            health -= damage;
            StartCoroutine(DamageEffects());

            if (health <= 0)
            {
                StartCoroutine(Disappear());
            }
        }
    }

    IEnumerator DamageEffects()
    {
        isTakingDamage = true;

        // L�ger mouvement de recul plus fort
        Vector3 originalPosition = transform.position;
        Vector3 knockbackPosition = transform.position - transform.forward * 0.5f; // Ajuster la distance de recul
        float knockbackDuration = 0.2f; // Augmenter la dur�e du recul

        // Mouvement de recul
        float elapsedTime = 0;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, knockbackPosition, elapsedTime / knockbackDuration);
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale * 0.9f, elapsedTime / knockbackDuration); // L�g�re d�formation
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Retourner � la position initiale
        elapsedTime = 0;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(knockbackPosition, originalPosition, elapsedTime / knockbackDuration);
            transform.localScale = Vector3.Lerp(initialScale * 0.9f, initialScale, elapsedTime / knockbackDuration); // Revenir � l'�chelle normale
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isTakingDamage = false;
    }

    IEnumerator Disappear()
    {
        isRotating = true;
        // Jouer le son de rotation
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        float elapsedTime = 0;
        while (elapsedTime < shrinkDuration)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
            transform.localScale = initialScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        AddXPToPlayer();
        Destroy(gameObject);
    }

    void FaceCamera()
    {
        // Tourner l'objet vers la cam�ra principale
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0; // Garder la rotation seulement sur l'axe Y
        transform.rotation = Quaternion.LookRotation(directionToCamera);
    }

    void AddXPToPlayer()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddXP(xpValue);
        }
    }
}
