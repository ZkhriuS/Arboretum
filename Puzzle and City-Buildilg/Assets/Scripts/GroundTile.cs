using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GroundTile : Tile
{
    private bool isFree;
    private bool isSetMode;
    public static Action<int> OnEnterGround;
    public static Action<int> OnExitGround;
    // Start is called before the first frame update
    void Start()
    {
        isFree = true;
        isSetMode = false;
    }

    public void SetNeighbours(ResourceTile prev)
    {
        foreach (Neighbours i in Enum.GetValues(typeof(Neighbours)))
        {
            if (gameObject.GetComponent<NeighbourController>().neighboursFree[i])
            {
                gameObject.GetComponent<NeighbourController>().neighboursFree[i].gameObject.GetComponent<NeighbourController>().neighboursFree[GetOppositeIndex((Neighbours)i)] = this;
                if (gameObject.GetComponent<NeighbourController>().neighboursFree[i].transform.IsChildOf(prev.transform))
                {
                    prev.transform.DetachChildren();
                    gameObject.GetComponent<NeighbourController>().neighboursFree[i].transform.SetParent(transform);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnterGround?.Invoke(-1);
    }
    private void OnTriggerExit(Collider other)
    {
        OnExitGround?.Invoke(+1);
    }

    public void InitSetMode()
    {
        isSetMode = true;
    }

    public void ExitSetMode()
    {
        isSetMode = false;
    }
}
