using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesOperator : MonoBehaviour
{
    [SerializeField] private GameObject cannotPayPanel;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private ObjectGenerator generator;
    [SerializeField] private GameManager manager;
    private int value;
    
    public bool IsMoneyOperationAvailable()
    {
        float calculation = float.Parse(resourceText.text) - value;
        return calculation >= 0;
    }

    private void DenyPayment()
    {
        if (!IsMoneyOperationAvailable())
        {
            cannotPayPanel.SetActive(true);
            generator.Back();
            manager.ChangeTileSetMode();
            value = 0;
        }
    }
    public void PayMoney()
    {
        float calculation = float.Parse(resourceText.text) - value;
        if (IsMoneyOperationAvailable())
        {
            resourceText.text = calculation.ToString();
        }
        else
            DenyPayment();

        value = 0;
    }
    public void CollectMoney(float addMoney)
    {
        resourceText.text = (float.Parse(resourceText.text)+addMoney).ToString();
    }

    public void SetValue(int price)
    {
        value = price;
    }
}
