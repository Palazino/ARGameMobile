using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFpsInTMP : MonoBehaviour
{
    [SerializeField]private TMP_Text _TextMeshPro;
    private float _fps;
    private float currentTime = 0;

    

    // Update is called once per frame
    void LateUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 1f)
        {
            _TextMeshPro.text = _fps.ToString("F2");
            currentTime = 0f;
        }
    }

    private void Update()
    {
         _fps = 1f / Time.deltaTime;
    }
}
