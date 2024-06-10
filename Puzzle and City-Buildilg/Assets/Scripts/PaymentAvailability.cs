using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaymentAvailability : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private List<TextMeshProUGUI> resources;
    public int moneyPrice;
    public List<int> resourcesPrice;

    private Button _buttonPay;
    // Start is called before the first frame update
    void Start()
    {
        _buttonPay = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        _buttonPay.interactable = HaveMoneyEnough() && HaveResourcesEnough();
    }

    private bool HaveMoneyEnough()
    {
        return int.Parse(money.text) - moneyPrice >= 0;
    }

    private bool HaveResourcesEnough()
    {
        bool result = true;
        for (int i=0; i< resources.Count; i++)
        {
            result = result && (int.Parse(resources[i].text) - resourcesPrice[i] >= 0);
        }

        return result;
    }
}
