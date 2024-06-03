using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    public Tile[] neighboursFree;
    private const int LENGTH = 6;
    private int neighboursMask;

    private void Start()
    {
        neighboursMask = 0;
        if(neighboursFree==null)
            neighboursFree = new Tile[LENGTH];
        UpdateNeighboursMask();
    }

    private void Update()
    {
        UpdateNeighboursMask();
    }

    public void SetNeighbour(int index, Tile target)
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

    private void UpdateNeighboursMask()
    {
        for (int i = 0; i < LENGTH; i++)
        {
            int value = 1 << i;
            neighboursMask += (neighboursFree[i])? value : -value;
        }
    }

    public void UnionMask(NeighbourController other)
    {
        neighboursMask = neighboursMask | other.neighboursMask;
        for (int i = 0; i < LENGTH; i++)
        {
            if (((1 << i) & neighboursMask) != 0 && !neighboursFree[i])
            {
                neighboursFree[i] = other.neighboursFree[i];
            }
        }
    }

}
