using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private Slider scaling;
    [SerializeField] private float minScale, maxScale;
    
    private Vector3 _lastPosition;

    private void Start()
    {
        var delta = minScale - maxScale;
        scaling.value = (sceneCamera.orthographicSize - maxScale) / delta;
    }

    public Vector3 GetSelectedMapPosition()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            if (!EventSystem.current.IsPointerOverGameObject() && 
                Physics.Raycast(sceneCamera.ScreenPointToRay(mousePos), out var hit, 100, placementLayerMask))
            {
                _lastPosition = hit.point;
            }
        }
        
        return _lastPosition;
    }

    public void Scale()
    {
        var delta = minScale - maxScale;
        sceneCamera.orthographicSize = maxScale + delta * (1 - scaling.value);
    }
}
