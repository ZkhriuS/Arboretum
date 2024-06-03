using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public bool isGrounded;
    public string type;
    
    public int GetOppositeIndex(Neighbours side)
    {
        switch (side)
        {
            case Neighbours.UP_LEFT: return (int)Neighbours.DOWN_RIGHT;
            case Neighbours.UP_CENTER: return (int)Neighbours.DOWN_CENTER;
            case Neighbours.UP_RIGHT: return (int)Neighbours.DOWN_LEFT;
            case Neighbours.DOWN_LEFT: return (int)Neighbours.UP_RIGHT;
            case Neighbours.DOWN_CENTER: return (int)Neighbours.UP_CENTER;
            case Neighbours.DOWN_RIGHT: return (int)Neighbours.UP_LEFT;
        }
        return -1;
    }
}

public enum Neighbours
{
    UP_LEFT,
    UP_CENTER,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_CENTER,
    DOWN_RIGHT
}