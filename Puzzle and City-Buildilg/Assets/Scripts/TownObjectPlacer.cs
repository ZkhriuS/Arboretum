using System.Collections;
using System.Collections.Generic;
using Edited;
using UnityEngine;
using UnityEngine.UI;
public class TownObjectPlacer : MonoBehaviour
{
    public float price;
    [SerializeField]
    private GameObject townExample;
    [SerializeField]
    private int platforms;
    private Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        GroundTile.OnEnterGround += ChangePlatformCount;
        GroundTile.OnExitGround += ChangePlatformCount;
        gameObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        Button apply = GameObject.Find("Yes").GetComponentInChildren<Button>();
        if (platforms == 0)
        {
            apply.interactable = true;
        }
        else
        {
            apply.interactable = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject apply = GameObject.Find("Yes");
        if (apply) { 
            Button applyButton = apply.GetComponentInChildren<Button>();
            if (platforms == 0)
            {
                applyButton.interactable = true;
            }
            else
            {
                applyButton.interactable = false;
            }
        }
        if (Input.GetMouseButton(0) && !Camera.main.GetComponentInChildren<MouseCameraDrag>().enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                
                Debug.Log(hit.collider.gameObject);
                basePosition = hit.point;
                gameObject.transform.position = new Vector3(basePosition.x, 0, basePosition.z);
            }
        }
    }

    /*private void OnMouseEnter()
    {
        Camera.main.GetComponentInChildren<MouseCameraDrag>().enabled = false;
    }

    private void OnMouseExit()
    {
        Camera.main.GetComponentInChildren<MouseCameraDrag>().enabled = true;
    }*/

    public void RotateTownObject(int direction)
    {
            if (direction > 0)
            {
                townExample.transform.Rotate(0, 0, 60);
            }
            else if (direction < 0)
            {
                townExample.transform.Rotate(0, 0, -60);
            }
    }

    private void ChangePlatformCount(int count)
    {
        platforms += count;
    }

    public void Apply()
    {
        GameObject town = gameObject.transform.parent.GetComponentInChildren<TownManager>().gameObject;
        GameObject townObject = Instantiate(townExample, town.gameObject.transform);
        townObject.transform.position = basePosition;
        TownManager townManager = town.GetComponent<TownManager>();
        if (townManager)
            townManager.ExitSetMode();
        Destroy(gameObject);
    }

    public void Cancel()
    {
        GameObject.Find("ShopButton").GetComponent<Button>().onClick.Invoke();
        GameObject town = gameObject.transform.parent.GetComponentInChildren<TownManager>().gameObject;
        TownManager townManager = town.GetComponent<TownManager>();
        if (townManager)
            townManager.ExitSetMode();
        Destroy(gameObject);
    }
}
