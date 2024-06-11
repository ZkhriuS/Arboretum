using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyOperator : MonoBehaviour
{
    [SerializeField] private GameObject cannotPayPanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private ObjectGenerator generator;
    [SerializeField] private GameManager manager;
    private int value;
    public bool IsMoneyOperationAvailable()
    {
        float calculation = float.Parse(moneyText.text) - value;
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
        float calculation = float.Parse(moneyText.text) - value;
        if (IsMoneyOperationAvailable())
        {
            moneyText.text = calculation.ToString();
        }
        else
            DenyPayment();

        value = 0;
    }
    public void CollectMoney(float addMoney)
    {
        moneyText.text = (float.Parse(moneyText.text)+addMoney).ToString();
    }

    public void SetValue(int price)
    {
        value = price;
    }
    
    public TextMeshProUGUI GetResourceText()
    {
        return moneyText;
    }
    
}
