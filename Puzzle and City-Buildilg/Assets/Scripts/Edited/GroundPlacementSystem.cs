using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementSystem : PlacementSystem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGroundGrid(GameObject tile, GameObject ground)
    {
        tileGrid.UpdateTileObject(ground, grid.WorldToCell(tile.gameObject.transform.position));
    }
}
