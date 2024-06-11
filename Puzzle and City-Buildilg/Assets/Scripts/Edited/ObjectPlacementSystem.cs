using System.Collections;
using System.Collections.Generic;
using Edited;
using UnityEngine;

public class ObjectPlacementSystem : PlacementSystem
{
    [SerializeField] private ObjectGenerator townGenerator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        touchIndicator = townGenerator.GetNewObject();
        if (touchIndicator)
        {
            Vector3 touchPosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(touchPosition);
            touchIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
        else
        {
            inputManager.ResetMapPosition();
        }
    }

    public void SetTownObject(GameObject townObject)
    {
        
        Instantiate(townObject, townObject.transform.parent);
        Destroy(townObject.gameObject);
        townGenerator.DeleteTownObject();
        touchIndicator = null;
    }
    
}
