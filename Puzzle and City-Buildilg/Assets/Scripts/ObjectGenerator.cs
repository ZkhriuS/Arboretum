using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ObjectGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject town;
    [SerializeField]
    private GameObject townObjectPrefab;
    public float price;

    public void  GenerateTownObject()
    {
        Instantiate(townObjectPrefab, town.transform);
        TownManager townManager = town.GetComponent<TownManager>();
        if(townManager)
            townManager.StartSetMode();
    }
}
