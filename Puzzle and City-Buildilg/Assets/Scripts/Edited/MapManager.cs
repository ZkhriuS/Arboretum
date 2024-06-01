using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Edited;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Edited.TileData;

public class MapManager : MonoBehaviour
{
    [SerializeField] private BonusTileData bonusTileData;
    private Dictionary<BonusTile, TileBase> bonusTileSet;
    [SerializeField] private List<GameObject> preparedTiles;

    [SerializeField] private Tilemap map;
    private TileBase newTile;
    private TileBase childTile;
    // Start is called before the first frame update
    void Start()
    {
        newTile = null;
        childTile = null;
        bonusTileSet = new Dictionary<BonusTile, TileBase>();
        for (int i = 0; i < bonusTileData.tiles.Length; i++)
        {
            bonusTileSet.Add(bonusTileData.bonusTile[i], bonusTileData.tiles[i]);
        }

        TileBase central = map.GetTile(new Vector3Int(0, 0, 0));
        preparedTiles[0].transform.position = central.GameObject().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewTiles()
    {
        int variant = Random.Range(0, bonusTileSet.Count);
        childTile = bonusTileSet.Values.ElementAt(variant);
    }
}
