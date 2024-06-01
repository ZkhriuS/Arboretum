using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;
    [SerializeField]
    private ChainManager chainManager;
    private List<GroundTile> ground;
    // Start is called before the first frame update
    void Start()
    {
        ground = new List<GroundTile>();
        ChainController.OnChainClosed += AddToGround;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSetMode()
    {
        ChainController[] chainControllers = tileGenerator.GetComponentsInChildren<ChainController>(true);
        foreach(var chainController in chainControllers)
        {
            chainController.enabled = false;
        }
        tileGenerator.enabled = false;
        chainManager.enabled = false;
        foreach(GroundTile tile in ground)
        {
            tile.InitSetMode();
        }

    }

    public void AddToGround(List<GroundTile> tiles)
    {
        ground.AddRange(tiles);
    }

    public void ExitSetMode()
    {
        foreach (GroundTile tile in ground)
        {
            tile.ExitSetMode();
        }
        chainManager.enabled = true;
        tileGenerator.enabled = true;
        ChainController[] chainControllers = tileGenerator.GetComponentsInChildren<ChainController>(true);
        foreach (var chainController in chainControllers)
        {
            chainController.enabled = true;
        }
    }
}
