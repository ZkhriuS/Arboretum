using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Edited;
using UnityEngine.UI;

public class ObjectGenerator : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> townObjectsPrefabs;
    [SerializeField]
    private GameObject town;

    [SerializeField] private ObjectHolder holder;

    private GameObject _newObject;

    [SerializeField] private ObjectPlacementSystem placement;

    public void  GenerateTownObject(int index)
    {
        _newObject = Instantiate(townObjectsPrefabs[index], town.transform);
        holder.SetTownObject(_newObject);
        
    }

    public GameObject GetNewObject()
    {
        return _newObject;
    }
    
    public void DeleteTownObject()
    {
        if (_newObject)
        {
            _newObject = null;
        }
    }

    public void Back()
    {
        if (_newObject)
        {
            Destroy(_newObject.gameObject);
        }
    }
    
    public void LeftRotate()
    {
        if (_newObject)
        {
            _newObject.gameObject.transform.Rotate(0, 0, -60);
        }
    }

    public void RightRotate()
    {
        if (_newObject)
        {
            _newObject.gameObject.transform.Rotate(0, 0, 60);
        }
    }
}
