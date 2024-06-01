using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager
{

    public static Action OnMouseCanvas;

    public static void SendTileSet()
    {
        if (OnMouseCanvas != null)
        {
            OnMouseCanvas.Invoke();
            Debug.Log("Invoked");
        }
    }
}
