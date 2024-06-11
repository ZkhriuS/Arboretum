using System;
using UnityEngine;

public class ResourceTile : Tile
{
    [SerializeField] private int oldSort;
    [SerializeField] private int newSort;
    
    private void Update()
    {
        renderer.sortingOrder = (isGrounded) ? newSort : oldSort;
    }
}
