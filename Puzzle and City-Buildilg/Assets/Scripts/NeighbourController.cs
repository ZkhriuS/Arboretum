using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    public Tile[] neighboursFree;
    private const int LENGTH = 6;

    private void Start()
    {
        if(neighboursFree==null)
            neighboursFree = new Tile[LENGTH];
    }

    public void SetNeigbour(int index, Tile target)
    {
        neighboursFree[index] = target;
    }

    public bool AllNeighboursResource()
    {
        for (int i = 0; i < 6; i++)
        {
            if (neighboursFree[i] && neighboursFree[i].GetComponent<ResourceTile>() == null)
                return false;
        }
        return true;
    }

    public int GetLength()
    {
        return LENGTH;
    }

}
