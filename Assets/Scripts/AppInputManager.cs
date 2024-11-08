using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AppInputManager : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] GameObject m_prefab;

    private void Awake()
    {
        m_raycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        m_camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch currentFinger = Input.GetTouch(0);
        var screenPosition = currentFinger.position;

        // Vérifier si l'objet touché a un script spécifique
        var ray = m_camera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GhostController ghost = hit.transform.GetComponent<GhostController>();
            if (ghost != null)
            {
                ghost.TakeDamage(10); // Appliquer des dégâts au fantôme
                return; // Ne pas créer un nouvel objet si on touche un objet existant
            }
        }

        // Raycast AR pour instancier un nouvel objet
        var raycastHits = new List<ARRaycastHit>();
        if (m_raycastManager.Raycast(screenPosition, raycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            ARRaycastHit firstHit = raycastHits[0];
            Pose hitPose = firstHit.pose;
            Vector3 spawnPosition = hitPose.position;
            Instantiate(m_prefab, spawnPosition, Quaternion.identity);
        }
    }
}
