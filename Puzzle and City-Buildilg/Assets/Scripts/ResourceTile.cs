using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : Tile
{
    [SerializeField]
    private Vector3 colliderSize;
    private bool isColliderModified;
    // Start is called before the first frame update
    void Start()
    {
        isColliderModified = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && !isColliderModified)
        {
            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            collider.size = colliderSize;
            isColliderModified = true;
        }
    }
}
