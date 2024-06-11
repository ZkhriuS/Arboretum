using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> resources;
    [SerializeField] private TextMeshProUGUI currentResource;

    [SerializeField] private List<TradeOperator> operators;
    [SerializeField] private List<TradeOperator> activeTrades;

    [SerializeField] private RectTransform content;

    [SerializeField] private Vector2 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        activeTrades = new List<TradeOperator>();
        for (int i = 0; i < 4; i++)
        {
            activeTrades.Add(Instantiate(operators[Random.Range(0,operators.Count)].gameObject, content).gameObject.GetComponent<TradeOperator>());
            spawnPosition = activeTrades[i].PlacePanel(spawnPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TradeOperator agreedTrade = CheckActiveTradesAgree();
        if (agreedTrade)
        {
            if(Trade(agreedTrade))
                GenerateNewTrade(agreedTrade);
        }
    }

    private TradeOperator CheckActiveTradesAgree()
    {
        foreach (var trade in activeTrades)
        {
            if (trade.IsAgree())
            {
                return trade;
            }
        }
        return null;
    }

    private void GenerateNewTrade(TradeOperator prevTrade)
    {
        spawnPosition = prevTrade.GetPanelPosition();
        activeTrades.Remove(prevTrade);
        Destroy(prevTrade.gameObject);
        foreach (var trade in activeTrades)
        {
            if (trade.GetPanelPosition().y < spawnPosition.y)
            {
                spawnPosition = trade.PlacePanel(spawnPosition);
            }
        }
        activeTrades.Add(Instantiate(operators[Random.Range(0,operators.Count)].gameObject, content).gameObject.GetComponent<TradeOperator>());
        spawnPosition = activeTrades.Last().PlacePanel(spawnPosition);
    }

    private bool Trade(TradeOperator trade)
    {
        TextMeshProUGUI temp = resources[(int) trade.GetDemand()];
        int value = int.Parse(temp.text);
        if (value < trade.GetDemandCount()) return false;
        ResourcesClick resourcePanelDemand = temp.gameObject.GetComponentInParent<ResourcesClick>();
        value -= trade.GetDemandCount();
        temp.text = value.ToString();
        if (CurrentClick.type.Equals(resourcePanelDemand.type)) 
            currentResource.text = temp.text;
        temp = resources[(int) trade.GetOffer()];
        ResourcesClick resourcePanelOffer = temp.gameObject.GetComponentInParent<ResourcesClick>();
        value = int.Parse(temp.text);
        value += trade.GetOfferCount();
        temp.text = value.ToString();
        if (CurrentClick.type.Equals(resourcePanelOffer.type)) 
            currentResource.text = temp.text;
        return true;
    }
}

