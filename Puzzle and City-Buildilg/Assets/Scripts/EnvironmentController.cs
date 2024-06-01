using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private Image indicImage;
    [SerializeField]
    private TextMeshProUGUI indicValue;
    [SerializeField]
    private float indicator;
    [SerializeField]
    private float personDamage;
    [SerializeField]
    private Button generatorButton;
    // Start is called before the first frame update
    void Start()
    {
        indicator = 100;
        indicImage.fillAmount = 1;
        indicValue.text = "100%";
        TileGenerator.TilePeopleArrived += ChangeIndicator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeIndicator(int people)
    {
        float damage = people * personDamage;
        if (indicator - damage > 0)
        {
            indicator -= damage;
            if (indicator < 10)
            {
                generatorButton.interactable = false;
            }
        }
        else
            indicator = 0;
        indicValue.text = (int)indicator + "%";
        indicImage.fillAmount = indicator/100;
    }
}
