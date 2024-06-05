using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour
    {
        public bool isGrounded;
        public string type;
    
        public static Neighbour GetOppositeIndex(Neighbour side)
        {
            switch (side)
            {
                case Neighbour.UP_LEFT: return Neighbour.DOWN_RIGHT;
                case Neighbour.UP_CENTER: return Neighbour.DOWN_CENTER;
                case Neighbour.UP_RIGHT: return Neighbour.DOWN_LEFT;
                case Neighbour.DOWN_LEFT: return Neighbour.UP_RIGHT;
                case Neighbour.DOWN_CENTER: return Neighbour.UP_CENTER;
                case Neighbour.DOWN_RIGHT: return Neighbour.UP_LEFT;
            }
            return side;
        }
    }
}