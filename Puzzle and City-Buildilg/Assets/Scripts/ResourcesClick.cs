using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesClick : MonoBehaviour
{
    public Image currentResourceImage;
    public TextMeshProUGUI currentScore;
    public string type;
    [SerializeField]
    private Sprite sprite;

    private void Start()
    {
        ChainManager.OnResourceUpdate += UpdateCurrentResource;
    }
    private void Update()
    {
    }
    public void ChangeResource()
    {
        currentResourceImage.sprite = sprite;
        currentScore.text = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        CurrentClick.type = type;
    }

    private void UpdateCurrentResource(string typeOther, string score)
    {
        if (CurrentClick.type.Equals(typeOther)){
            currentScore.text = score;
        }
    }
}
