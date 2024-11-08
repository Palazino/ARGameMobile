using UnityEngine;

public class RotationOnTap : MonoBehaviour
{
    private float rotationSpeed = 0f;
    private float maxRotationSpeed = 360f; // Vitesse de rotation initiale
    private bool isRotating = false;
    private AudioSource audioSource;
    private Vector3 initialScale;

    void Start()
    {
        // Obtenir le composant AudioSource de l'objet
        audioSource = GetComponent<AudioSource>();
        // Enregistrer l'échelle initiale
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        StartRotationAndShrink();
                    }
                }
            }
        }

        if (isRotating)
        {
            // Appliquer la rotation
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Réduire progressivement la vitesse de rotation
            rotationSpeed = Mathf.Lerp(rotationSpeed, 0, Time.deltaTime);

            // Réduire progressivement l'échelle pour rendre l'objet microscopique
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);

            // Arrêter la rotation et détruire l'objet lorsque la vitesse est presque nulle
            if (rotationSpeed < 0.1f && transform.localScale.magnitude < 0.01f)
            {
                isRotating = false;
                rotationSpeed = 0f;
                Destroy(gameObject);
            }
        }
    }

    void StartRotationAndShrink()
    {
        rotationSpeed = maxRotationSpeed;
        isRotating = true;
        // Jouer le son de rotation
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
