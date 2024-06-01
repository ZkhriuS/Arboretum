using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChainManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chainPanel;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    private Sprite[] sprites;
    private ChainController chainController;
    public GameObject[] resourcePanels;
    public GameObject currentResourcePanel;
    public static Action<string, string> OnResourceUpdate;
    private bool isInfoOpen;
    // Start is called before the first frame update
    void Start()
    {
        isInfoOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<TileGenerator>().IsOtherActionBlocked() && !isInfoOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.MaxValue))
                {
                    ChainController temp = hit.collider.gameObject.GetComponent<ChainController>();
                    if (temp)
                    {
                        chainController = temp;
                        isInfoOpen = true;
                        Info();
                        chainPanel.SetActive(true);
                    }
                }
            }
        }
    }

    private void Info()
    {
        text.text = "Вы получите " + chainController.score + "    \t.\nЗамкнуть цепочку?";
        Image image = text.GetComponentInChildren<Image>();
        switch (chainController.GetChainType())
        {
            case "factory": image.sprite = sprites[0];
                break;
            case "forest":
                image.sprite = sprites[1];
                break;
            case "harvest":
                image.sprite = sprites[2];
                break;
            case "house":
                image.sprite = sprites[3];
                break;
            case "services":
                image.sprite = sprites[4];
                break;
            case "water":
                image.sprite = sprites[5];
                break;
        }
    }

    public void OnButtonYes()
    {
        if(chainController)
        {
            UpdateResourceScore(chainController.GetChainType(), chainController.score);
            chainController.CloseChain();
        }
        isInfoOpen = false;
    }

    public void OnButtonNo()
    {
        chainController = null;
        isInfoOpen = false;
    }

    private void UpdateResourceScore(string type, int score)
    {
        TextMeshProUGUI value = null;
        switch (type)
        {
            case "factory":
                {
                    value = resourcePanels[0].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();

                }
                break;
            case "forest":
                {
                    value = resourcePanels[1].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();
                }
                break;
            case "harvest":
                {
                    value = resourcePanels[2].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();
                }
                break;
            case "house":
                {
                    value = resourcePanels[3].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();
                }
                break;
            case "services":
                {
                    value = resourcePanels[4].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();
                }
                break;
            case "water":
                {
                    value = resourcePanels[5].GetComponentInChildren<TextMeshProUGUI>();
                    value.text = (int.Parse(value.text) + score).ToString();
                }
                break;
        }
        OnResourceUpdate?.Invoke(type, value.text);
    }
}
