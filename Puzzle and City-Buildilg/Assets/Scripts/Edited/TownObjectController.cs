using System;
using System.Collections;
using System.Collections.Generic;
using Money;
using UnityEngine;

public class TownObjectController : MonoBehaviour
{
    public static event Action<float> TownObjectPlaced;
    [SerializeField] private float damage;
    private bool placeAvailable;
    [SerializeField] private bool isPlaced;

    private List<GameObject> triggered;
    // Start is called before the first frame update
    void Start()
    {
        triggered=new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
            placeAvailable = HaveTriggeredGround();
    }

    private void OnCollisionEnter(Collision other)
    {
        triggered.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        if(!isPlaced)
            triggered.Remove(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        triggered.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!isPlaced)
         triggered.Remove(other.gameObject);
    }

    public bool IsPlaceAvailable()
    {
        return placeAvailable;
    }

    private bool HaveTriggeredGround()
    {
        placeAvailable = true;
        //if (triggered.Count == 0) return false;
        foreach (var obj in triggered)
        {
            GroundTile groundTile = obj.GetComponent<GroundTile>();
            if(groundTile)
                placeAvailable = placeAvailable && obj.GetComponent<GroundTile>().isFree;
            else
            {
                placeAvailable = false;
            }
        }
        return placeAvailable;
    }

    public void SetPlaced()
    {
        isPlaced = true;
        foreach (var obj in triggered)
        {
            obj.GetComponent<GroundTile>().isFree=false;
        }
        TownObjectPlaced?.Invoke(damage);
        
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}
