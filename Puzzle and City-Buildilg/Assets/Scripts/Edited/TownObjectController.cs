using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownObjectController : MonoBehaviour
{
    private bool placeAvailable;
    private bool isPlaced;

    private List<GameObject> triggered;
    // Start is called before the first frame update
    void Start()
    {
        triggered=new List<GameObject>();
        isPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
            placeAvailable = HaveTriggeredGround();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject + " Enter");
        triggered.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log(other.gameObject + " Exit");
        triggered.Remove(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject + " Enter");
        triggered.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject + " Exit");
        triggered.Remove(other.gameObject);
    }

    public bool IsPlaceAvailable()
    {
        return placeAvailable;
    }

    private bool HaveTriggeredGround()
    {
        placeAvailable = true;
        foreach (var obj in triggered)
        {
            placeAvailable = placeAvailable && obj.GetComponent<GroundTile>();
        }

        return placeAvailable;
    }

    public void SetPlaced()
    {
        isPlaced = true;
    }
}
