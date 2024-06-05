using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;

public class NeighbourController : MonoBehaviour
{
    [FormerlySerializedAs("neighboursFree")]
    public SerializedDictionary<Neighbour, Tile> neighbours = new ()
    {
        {Neighbour.UP_LEFT, null},
        {Neighbour.UP_CENTER, null},
        {Neighbour.UP_RIGHT, null},
        {Neighbour.DOWN_RIGHT, null},
        {Neighbour.DOWN_CENTER, null},
        {Neighbour.DOWN_LEFT, null},
    };
    
    public void SetNeighbour(Neighbour neighbour, Tile target)
    {
        neighbours[neighbour] = target;
    }
}
