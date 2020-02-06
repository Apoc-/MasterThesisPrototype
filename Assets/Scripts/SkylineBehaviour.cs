using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkylineBehaviour : MonoBehaviour
{
    public GameObject farContainer;
    public GameObject medContainer;
    public GameObject closeContainer;

    public float farFactor = 0;
    public float medFactor = 0.5f;
    public float closeFactor = 1;

    private Vector3 _lastCameraPosition;
    
    void Start()
    {
        _lastCameraPosition = Camera.main.transform.position;
    }
    
    void Update()
    {
        var currentCamPos = Camera.main.transform.position;
        var deltaX = currentCamPos.x - _lastCameraPosition.x;
        _lastCameraPosition = currentCamPos;

        UpdateHorizontalPosition(farContainer, farFactor, -deltaX);
        UpdateHorizontalPosition(medContainer, medFactor, -deltaX);
        UpdateHorizontalPosition(closeContainer, closeFactor, -deltaX);

    }

    private void UpdateHorizontalPosition(GameObject go, float changeFactor, float horizontalDelta)
    {
        var pos = go.transform.position;
        pos.x += horizontalDelta * changeFactor;

        go.transform.position = pos;
    }
}
