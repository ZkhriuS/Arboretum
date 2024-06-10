using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PeopleOperator : MonoBehaviour
{
    public static event Action<int> TilePeopleArrived;
    [SerializeField] private TextMeshProUGUI peopleText;

    [SerializeField]
    private int minPeople;
    [SerializeField]
    private int maxPeople;

    public void PeopleArrive()
    {
        int count = UnityEngine.Random.Range(minPeople, maxPeople);
        TilePeopleArrived?.Invoke(count);
        peopleText.text = (int.Parse(peopleText.text) + count).ToString();
    }
}
