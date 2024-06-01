using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Button> setTileButtons;

    [SerializeField] private List<Button> gameManagementButtons;

    private bool isTileSet; 
    // Start is called before the first frame update
    void Start()
    {
        isTileSet = false;
        SetInteraction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTileSetMode()
    {
        isTileSet = !isTileSet;
        SetInteraction();
    }

    private void SetInteraction()
    {
        foreach (var button in setTileButtons)
        {
            button.interactable = isTileSet;
        }

        foreach (var button in gameManagementButtons)
        {
            button.interactable = !isTileSet;
        }
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    public Button FindSelectButton()
    {
        Button selectButton = GameObject.FindWithTag("Select").GetComponent<Button>();
        return selectButton;
    }

    public Button FindScaleButton()
    {
        Button selectButton = GameObject.FindWithTag("Scale").GetComponent<Button>();
        return selectButton;
    }
}
