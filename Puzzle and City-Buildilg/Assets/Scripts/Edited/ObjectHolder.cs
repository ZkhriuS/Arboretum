using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private GameObject _townObject;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ObjectPlacementSystem placement;

    [SerializeField] private MoneyOperator moneyOperator;

    [SerializeField] private List<MoneyOperator> resourcesOperators;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTownObject(GameObject townObject)
    {
        _townObject = townObject;
    }
    
    public void Apply()
    {
        if (_townObject != null)
        {
            TownObjectController controller = _townObject.GetComponent<TownObjectController>();
            if (controller.IsPlaceAvailable())
            {
                controller.SetPlaced();
                placement.SetTownObject(_townObject);
                placement.ResetTouchIndicator();
                gameManager.ChangeTileSetMode();
                moneyOperator.PayMoney();
                foreach (var resourcesOperator in resourcesOperators)
                {
                    resourcesOperator.PayMoney();
                }
            }
        }
            
    }
}
