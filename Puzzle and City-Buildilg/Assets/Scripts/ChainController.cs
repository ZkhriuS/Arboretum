using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChainController : MonoBehaviour
{
    public int score;
    public GameObject groundTile;
    [SerializeField]
    private GameObject scoreTextPrefab;
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private List<ResourceTile> chain;
    private bool closed;
    public static Action<List<GroundTile>> OnChainClosed;

    // Start is called before the first frame update
    void Start()
    {
        chain = new List<ResourceTile>();
        closed = false;
        chain.Add(gameObject.GetComponent<ResourceTile>());
        ResourceTile tile = gameObject.GetComponent<ResourceTile>();
        NeighbourController tileNeighbourC = gameObject.GetComponent<NeighbourController>();
        for (int i = 0; i < 6; i++)
        {
            if (tileNeighbourC.neighboursFree[i])
            {
                if (tileNeighbourC.neighboursFree[i].GetComponent<ResourceTile>() && tile.type.Equals(tileNeighbourC.neighboursFree[i].type))
                {
                    foreach(var item in tileNeighbourC.neighboursFree[i].GetComponent<ChainController>().chain)
                    {
                        if (!chain.Contains(item))
                        {
                            chain.Add(item);
                        }
                    }
                }
            }
        }
        if (gameObject.GetComponent<Tile>().isGrounded)
            AddCanvas();
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
                GameObject tile = Instantiate(groundTile, item.GetComponentInParent<TileGenerator>().gameObject.transform);
                tile.transform.position = tilePosition;
                tile.GetComponent<NeighbourController>().neighboursFree = item.gameObject.GetComponent<NeighbourController>().neighboursFree;
                tile.GetComponent<GroundTile>().SetNeighbours(item);
                ground.Add(tile.GetComponent<GroundTile>());
                Destroy(item.gameObject);
            }
            OnChainClosed?.Invoke(ground);
        }
    }
    private void OnMouseEnter()
    {
        if (scoreText && !gameObject.GetComponent<BoxCollider>().isTrigger)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = score.ToString();
        }
    }
    private void OnMouseExit()
    {
        if(scoreText && !gameObject.GetComponent<BoxCollider>().isTrigger)
            scoreText.gameObject.SetActive(false);
    }
    public int IncreaseScore(BonusTile prev)
    {
        ResourceTile tile = gameObject.GetComponent<ResourceTile>();
        NeighbourController tileNeighbourC = gameObject.GetComponent<NeighbourController>();
        int value = 0;
        for (int i=0; i<6; i++)
        {
            if (tileNeighbourC.neighboursFree[i])
            {
                if (tileNeighbourC.neighboursFree[i].GetComponent<ResourceTile>() && tile.type.Equals(tileNeighbourC.neighboursFree[i].type))
                {
                    if (value != tileNeighbourC.neighboursFree[i].GetComponent<ChainController>().score)
                    {
                        score += tileNeighbourC.neighboursFree[i].GetComponent<ChainController>().score;
                        value = tileNeighbourC.neighboursFree[i].GetComponent<ChainController>().score;
                    }
                    foreach (var item in tileNeighbourC.neighboursFree[i].GetComponent<ChainController>().chain)
                    {
                        if (!chain.Contains(item))
                        {
                            chain.Add(item);
                        }
                    }
                }
            }
        }
        if (prev.typeBonus == '+' && tile.type.Equals(prev.type))
        {
            score += prev.bonus;
        } else if(prev.typeBonus == '*' && tile.type.Equals(prev.type))
        {
            score *= prev.bonus;
        }
        foreach(var item in chain)
        {
            item.GetComponent<ChainController>().score = score;
            if(item!=tile)
                item.GetComponent<ChainController>().chain.Add(tile);
        }
        return score;
    }

    public int GetScore()
    {
        return score;
    }
    public void AddCanvas()
    {
        scoreTextPrefab.GetComponent<Canvas>().worldCamera = Camera.main;
        GameObject canvas = Instantiate(scoreTextPrefab, gameObject.transform);
        scoreText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        scoreText.gameObject.SetActive(true);
        scoreText.text = score.ToString();
        canvas.transform.position = gameObject.transform.position;
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
        return chain[0].type;
    }
}
