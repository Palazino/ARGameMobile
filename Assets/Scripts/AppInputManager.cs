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

    // Update is called once per frame
    void Update()
    {
        if  ((Input.touchCount == 0)) return;

        Touch currentFinger = Input.GetTouch(0);
        var screenPosition = currentFinger.position;
        var ray = m_camera.ScreenPointToRay(screenPosition);
        var raycastHits = new List<ARRaycastHit>();

        if (m_raycastManager.Raycast(ray, raycastHits, trackableTypes: UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            ARRaycastHit firstHit = raycastHits[0];
            Pose hitPose = firstHit.pose;
            Vector3 spawnPosition = hitPose.position;
            Instantiate(m_prefab,spawnPosition, Quaternion.identity);
        }
    }
}
