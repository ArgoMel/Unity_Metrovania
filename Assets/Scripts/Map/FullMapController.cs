using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapController : MonoBehaviour
{
    [SerializeField] private InputRender input;

    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
    public float moveModifier=1f;

    private MapCameraController mapCam;
    private Camera cam;
    private Vector2 moveDir;
    private int dir=1;
    private bool isZooming;

    private void Awake()
    {
        mapCam=transform.parent.GetComponentInChildren<MapCameraController>();  
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        transform.position=mapCam.transform.position;
        input.MoveEvent += OnMove;
        input.ZoomInMapEvent += OnZoomInMap;
        input.ZoomInMapCanceledEvent += OnZoomMapCanceled;
        input.ZoomOutMapEvent += OnZoomOutMap;
        input.ZoomOutMapCanceledEvent += OnZoomMapCanceled;
    }

    private void OnDisable()
    {
        input.MoveEvent -= OnMove;
        input.ZoomInMapEvent -= OnZoomInMap;
        input.ZoomInMapCanceledEvent -= OnZoomMapCanceled;
        input.ZoomOutMapEvent -= OnZoomOutMap;
        input.ZoomOutMapCanceledEvent -= OnZoomMapCanceled;
    }

    private void Update()
    {
        transform.position += 
            (Vector3)moveDir*cam.orthographicSize * Time.unscaledDeltaTime* moveModifier;
        if (isZooming) 
        {
            cam.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime* dir;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize,minZoom,maxZoom);
        }
    }

    private void OnMove(Vector2 vector)
    {
        moveDir = vector;
    }

    private void OnZoomInMap()
    {
        isZooming = true;
        dir = 1;
    }

    private void OnZoomOutMap()
    {
        isZooming = true;
        dir = -1;
    }

    private void OnZoomMapCanceled()
    {
        isZooming = false;
    }
}
