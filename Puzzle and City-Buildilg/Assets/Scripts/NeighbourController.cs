using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    public SerializedDictionary<Neighbours, Tile> neighboursFree = new ()
    {
        {Neighbours.UP_LEFT, null},
        {Neighbours.UP_CENTER, null},
        {Neighbours.UP_RIGHT, null},
        {Neighbours.DOWN_RIGHT, null},
        {Neighbours.DOWN_CENTER, null},
        {Neighbours.DOWN_LEFT, null},
    };
    
    // public List<Tile> neighboursFree = new (new Tile[LENGTH]);

    public void SetNeighbour(Neighbours neighbour, Tile target)
    {
        neighboursFree[neighbour] = target;
    }
}
