using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour
{
    public int health = 100; // Points de vie du fantôme
    public int xpValue = 50; // Valeur en XP du fantôme
    private float maxRotationSpeed = 360f; // Vitesse de rotation initiale
    private bool isRotating = false;
    private AudioSource audioSource;
    private Vector3 initialScale;
    private bool isTakingDamage = false;
    private float moveSpeed = 0.5f; // Vitesse de déplacement vers la caméra
    private float shrinkDuration = 2.0f; // Durée du rétrécissement en secondes

    public float damageDistance = 1.0f; // Distance à partir de laquelle le joueur prend des dégâts
    public int damageAmount = 10; // Dégâts infligés au joueur
    public float damageInterval = 1.0f; // Intervalle de temps entre chaque dégât infligé au joueur
    private bool isAtDamageDistance = false; // Indique si le fantôme est à la distance de dégâts

    private PlayerHealth playerHealth; // Référence au script de la santé du joueur

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale;
        playerHealth = FindObjectOfType<PlayerHealth>(); // Trouver le script PlayerHealth
    }

    void Update()
    {
        // Tourner constamment vers la caméra
        FaceCamera();

        // Avancer vers la caméra ou vérifier la distance
        if (!isAtDamageDistance)
        {
            MoveTowardsCamera();
            CheckPlayerDistance();
        }

        if (isRotating)
        {
            // Appliquer la rotation
            transform.Rotate(Vector3.up, maxRotationSpeed * Time.deltaTime);
        }
    }

    void MoveTowardsCamera()
    {
        if (!isTakingDamage && !isRotating) // Avancer seulement quand il ne prend pas de coups et ne disparaît pas
        {
            Vector3 directionToCamera = (Camera.main.transform.position - transform.position).normalized;
            transform.position += directionToCamera * moveSpeed * Time.deltaTime;
        }
    }

    void CheckPlayerDistance()
    {
        if (playerHealth != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);
            if (distanceToPlayer <= damageDistance)
            {
                if (!isAtDamageDistance)
                {
                    isAtDamageDistance = true;
                    StartCoroutine(DealDamageOverTime());
                }
            }
            else
            {
                isAtDamageDistance = false;
                StopCoroutine(DealDamageOverTime());
            }
        }
    }

    IEnumerator DealDamageOverTime()
    {
        while (isAtDamageDistance)
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
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

        // Léger mouvement de recul plus fort
        Vector3 originalPosition = transform.position;
        Vector3 knockbackPosition = transform.position - transform.forward * 0.5f; // Ajuster la distance de recul
        float knockbackDuration = 0.2f; // Augmenter la durée du recul

        // Mouvement de recul
        float elapsedTime = 0;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, knockbackPosition, elapsedTime / knockbackDuration);
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale * 0.9f, elapsedTime / knockbackDuration); // Légère déformation
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Retourner à la position initiale
        elapsedTime = 0;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(knockbackPosition, originalPosition, elapsedTime / knockbackDuration);
            transform.localScale = Vector3.Lerp(initialScale * 0.9f, initialScale, elapsedTime / knockbackDuration); // Revenir à l'échelle normale
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isTakingDamage = false;
        isAtDamageDistance = false; // Recommencer à se déplacer après avoir été repoussé
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

        RemoveGhostFromList(); // Supprimer le fantôme de la liste active
        Destroy(gameObject);
    }

    void FaceCamera()
    {
        // Tourner l'objet pour toujours faire face à la caméra principale
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

    void RemoveGhostFromList()
    {
        GhostSpawner spawner = FindObjectOfType<GhostSpawner>();
        if (spawner != null)
        {
            spawner.RemoveGhost(this.gameObject);
        }
    }
}
