using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public bool isGrounded;
    public string type;
    [SerializeField] protected SpriteRenderer renderer;
    
    public Neighbours GetOppositeIndex(Neighbours side)
    {
        switch (side)
        {
            case Neighbours.UP_LEFT: return Neighbours.DOWN_RIGHT;
            case Neighbours.UP_CENTER: return Neighbours.DOWN_CENTER;
            case Neighbours.UP_RIGHT: return Neighbours.DOWN_LEFT;
            case Neighbours.DOWN_LEFT: return Neighbours.UP_RIGHT;
            case Neighbours.DOWN_CENTER: return Neighbours.UP_CENTER;
            case Neighbours.DOWN_RIGHT: return Neighbours.UP_LEFT;
        }
        return side;
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