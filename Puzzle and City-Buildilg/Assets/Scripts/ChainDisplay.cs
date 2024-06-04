using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChainDisplay : MonoBehaviour
{
    public ChainController chainController;
    [Space]
    public RectTransform chainScore;

    private void OnEnable()
    {
        chainController.OnChainChanged += Display;
    }
    
    private void OnDisable()
    {
        chainController.OnChainChanged -= Display;
    }
    
    public void Display()
    {
        foreach (var chainDisplay in chainController.chain.Select(tile => tile.GetComponent<ChainDisplay>()))
        {
            chainDisplay.ShowScore(false);
        }

        chainScore.GetComponentInChildren<TextMeshProUGUI>().text = chainController.score.ToString();
        ShowScore(true);
    }

    public void ShowScore(bool show)
    {
        chainScore.gameObject.SetActive(show);
    }
}