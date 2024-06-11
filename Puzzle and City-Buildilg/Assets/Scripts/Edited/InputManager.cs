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
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private LayerMask ignoreLayerMask;
        [SerializeField] private Slider scaling;
    [SerializeField] private float minScale, maxScale;

    private void Start()
    {
        float delta = minScale - maxScale;
        scaling.value = (sceneCamera.orthographicSize - maxScale) / delta;
    }

    public Vector3 GetSelectedMapPosition()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.touches[0].position;
            touchPos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(touchPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
            {
                lastPosition = hit.point;
            }
        }
        
        
#if UNITY_EDITOR
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = sceneCamera.nearClipPlane;
                Ray ray = sceneCamera.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, 100, placementLayerMask))
                {
                    lastPosition = hit.point;
                }
            }
        }
#endif
        return lastPosition;
    }

    public void Scale()
    {
        float delta = (minScale - maxScale);
        sceneCamera.orthographicSize = maxScale+delta * (1-scaling.value);
    }

    public void ResetMapPosition()
    {
        lastPosition = new Vector3(0, 0, 20);
    }
}
