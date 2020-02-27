using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float minZoomSize = 40;
    public float maxZoomSize = 200;
    public float initialZoom = 100;
    public float zoomStep = 10;
    public float zoomDuration = 10;
    public Vector3 PermanentOffset;
    public AnimationCurve zoomAnimationCurve = AnimationCurve.Linear(0f,0f,1f,1f);
    
    private Camera Cam => gameObject.gameObject.GetComponent<Camera>();
    private float CurrentZoom => Cam.orthographicSize;
    private float _zoomTime = 0;
    private float _zoomStart;
    private float _zoomTarget;

    public float cameraMinYOffset = 25f;
    public float cameraMaxYOffset = 100f;
    public float cameraMinY = -3.5f;
    public float cameraMaxY = 10f;

    private void Start()
    {
        Cam.orthographicSize = initialZoom;
        _zoomTarget = initialZoom;
        _zoomStart = initialZoom;
    }

    void Update()
    {
        HandleZoom();
        FollowPlayer();
    }

    private void HandleZoom()
    {
        _zoomTarget = Mathf.Clamp(_zoomTarget, minZoomSize, maxZoomSize);
        _zoomTime += Time.deltaTime;
        var prog = _zoomTime / zoomDuration;
        Cam.orthographicSize = Mathf.Lerp(_zoomStart, _zoomTarget, zoomAnimationCurve.Evaluate(prog));
    }

    private void FollowPlayer()
    {
        var targetPos = GameManager.Instance.player.transform.position;
        targetPos.z = -50;
        var zoom = (CurrentZoom-minZoomSize) / (maxZoomSize-minZoomSize);
        targetPos.y += Mathf.Lerp(cameraMinYOffset, cameraMaxYOffset, zoom);
        targetPos.y = Mathf.Clamp(targetPos.y, cameraMinY, cameraMaxY);
        targetPos += PermanentOffset;
        transform.position = targetPos;
    }

    public void ZoomIn()
    {
        _zoomStart = Cam.orthographicSize;
        _zoomTime = 0;
        _zoomTarget -= zoomStep;
    }

    public void ZoomOut()
    {
        _zoomStart = Cam.orthographicSize;
        _zoomTime = 0;
        _zoomTarget += zoomStep;
    }
}
