using System.Collections.Generic;
using UnityEngine;
using System;
using Edited;
using Tiles;

public class ChainController : MonoBehaviour
{
    public int score;
    public GameObject groundTile;
    public List<ChainController> chain; // TODO ResourceTile -> ChainController
    private bool closed;
    public static Action<List<GroundTile>> OnChainClosed;
    public Action OnChainChanged;

    // Start is called before the first frame update
    void Start()
    {
        chain = new List<ChainController>();
        closed = false;
        chain.Add(gameObject.GetComponent<ChainController>());
        ResourceTile tile = gameObject.GetComponent<ResourceTile>();
        NeighbourController tileNeighbourC = gameObject.GetComponent<NeighbourController>();
        foreach (Neighbour i in Enum.GetValues(typeof(Neighbour)))
        {
            if (tileNeighbourC.neighbours[i])
            {
                if (tileNeighbourC.neighbours[i].GetComponent<ResourceTile>() && tile.type.Equals(tileNeighbourC.neighbours[i].type))
                {
                    foreach(var item in tileNeighbourC.neighbours[i].GetComponent<ChainController>().chain)
                    {
                        if (!chain.Contains(item))
                        {
                            chain.Add(item);
                        }
                    }
                }
            }
        }
        OnChainChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(closed)
        {
            List<GroundTile> ground = new List<GroundTile>();
            foreach(var item in chain)
            {
                Vector3 tilePosition = item.gameObject.transform.position;
                GameObject tile = Instantiate(groundTile, item.GetComponentInParent<TileHolder>().gameObject.transform);
                TileHolder tileHolder = gameObject.GetComponentInParent<TileHolder>();
                tileHolder.UpdateGrid(item.GetComponent<Tile>().gameObject, tile);
                tile.transform.position = tilePosition;
                tile.GetComponent<NeighbourController>().neighbours = item.gameObject.GetComponent<NeighbourController>().neighbours;
                tile.GetComponent<GroundTile>().SetNeighbours(item.GetComponent<ResourceTile>());
                ground.Add(tile.GetComponent<GroundTile>());
                Destroy(item.gameObject);
            }
            OnChainClosed?.Invoke(ground);
        }
    }

    public int IncreaseScore(BonusTile stackedTile)
    {
        var tileType = GetComponent<ResourceTile>().type;

        foreach (var neighbour in GetComponent<NeighbourController>().neighbours)
        {
            if (neighbour.Value == null) continue;

            if (!neighbour.Value.TryGetComponent(out ResourceTile resourceTile)) continue;
            if (resourceTile.type != tileType) continue;

            if (!neighbour.Value.TryGetComponent(out ChainController chainElement)) continue;
            if (chain == chainElement.chain) continue;

            var newScore = chainElement.score + score;

            chainElement.chain.AddRange(chain);
            
            foreach (var element in chainElement.chain)
            {
                element.chain = chainElement.chain;
                element.score = newScore;
            }
        }

        if (tileType == stackedTile.type)
        {
            var newStackedScore = stackedTile.typeBonus switch
            {
                '+' => score + stackedTile.bonus,
                '*' => score * stackedTile.bonus,
                _ => score
            };

            foreach (var element in chain)
            {
                element.score = newStackedScore;
            }
        }

        OnChainChanged?.Invoke();

        return score;
    }

    public void CloseChain()
    {
        closed = true;
        foreach(var item in chain)
        {
            if (item.gameObject.GetComponentInChildren<Canvas>())
            {
                GameObject canvasObject = item.gameObject.GetComponentInChildren<Canvas>().gameObject;
                Destroy(canvasObject);
            }
        }
    }

    public string GetChainType()
    {
        return chain[0].GetComponent<ResourceTile>().type;
    }
}
