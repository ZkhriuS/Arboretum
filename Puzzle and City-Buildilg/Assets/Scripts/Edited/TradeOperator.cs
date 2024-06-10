using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeOperator : MonoBehaviour
{
    [SerializeField] private ResourcesType demand;

    [SerializeField] private ResourcesType offer;

    [SerializeField] private int demandCount;

    [SerializeField] private int offerCount;

    [SerializeField] private GameObject panel;

    [SerializeField] private float offset;

    private bool agree;
    // Start is called before the first frame update
    void Start()
    {
        agree = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 PlacePanel(Vector2 position)
    {
        RectTransform panelTransform = panel.gameObject.GetComponent<RectTransform>();
        panelTransform.anchoredPosition = position;
        Vector2 nextPosition = position - new Vector2(0, panelTransform.rect.height + offset);
        return nextPosition;
    }

    public Vector2 GetPanelPosition()
    {
        RectTransform panelTransform = panel.gameObject.GetComponent<RectTransform>();
        return panelTransform.anchoredPosition;
    }

    public void SetAgree()
    {
        agree = true;
    }

    public bool IsAgree()
    {
        return agree;
    }

    public ResourcesType GetDemand()
    {
        return demand;
    }

    public ResourcesType GetOffer()
    {
        return offer;
    }

    public int GetDemandCount()
    {
        return demandCount;
    }

    public int GetOfferCount()
    {
        return offerCount;
    }
}
