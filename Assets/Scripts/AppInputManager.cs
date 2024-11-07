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

        // Vérification du toucher pour changer la couleur
        var ray = m_camera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Si l'objet touché a le script ChangeColorOnTap, on ne fait rien d'autre
            if (hit.transform.GetComponent<ChangeColorOnTap>() != null)
            {
                return;
            }
        }

        // Sinon, on effectue un raycast AR pour instancier un nouveau cube
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
