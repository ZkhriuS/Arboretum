using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private Image indicImageGood;

    [SerializeField] private Image indicImageBad;
    [SerializeField]
    private TextMeshProUGUI indicValue;
    [SerializeField]
    private float indicator;
    [SerializeField]
    private float personDamage;
    [SerializeField]
    private Button generatorButton;
    [SerializeField]
    private Vector2 maxScale;
    [SerializeField]
    private Vector2 minScale;
    // Start is called before the first frame update
    void Start()
    {
        indicator = 100;
        indicValue.text = "100%";
        PeopleOperator.TilePeopleArrived += ChangeIndicatorPeople;
        TownObjectController.TownObjectPlaced += ChangeIndicatorHarm;
        UpdateIndic();
    }

    // Update is called once per frame
    void Update()
    {
        if (indicator < 10)
        {
            generatorButton.interactable = false;
        }
    }

    private void ChangeIndicatorPeople(int people)
    {
        float damage = people * personDamage;
        ChangeIndicatorHarm(damage);
    }

    private void ChangeIndicatorHarm(float damage)
    {
        if (indicator - damage > 0)
        {
            indicator -= damage;
            if(indicator>=10)
            {
                generatorButton.interactable = true;
            }
        }
        else
            indicator = 0;
        indicValue.text = (int)indicator + "%";
        UpdateIndic();
    }

    private void UpdateIndic()
    {
        float value = indicator / 100;
        float side = minScale.x + value * (maxScale.x - minScale.x);
        indicImageBad.gameObject.GetComponent<RectTransform>().localScale = new Vector3(side, side);
        indicImageGood.gameObject.GetComponent<RectTransform>().localScale = new Vector3(side, side);
        indicImageBad.color = new Color(1, 1, 1, 1 - value);
        indicImageGood.color = new Color(1, 1, 1, value);
    }
}
