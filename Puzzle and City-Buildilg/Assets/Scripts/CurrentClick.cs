using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentClick : MonoBehaviour
{
    public GameObject resourcesPanel;
    private bool active;
    public static string type = "house";
    private void Start()
    {
    }
    private void Update()
    {
        
    }
    public void ChangeActive()
    {
        active = !active;
        resourcesPanel.SetActive(active);
    }

}
