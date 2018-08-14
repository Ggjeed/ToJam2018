using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapHandler : MonoBehaviour {

    // Prefabs
    public GameObject tile;

    public int Rows;
    public int Columns;

    public List<GameObject> Tiles;
    public List<GameObject> StartTiles;
    public int PixelSize = 32;

    private int NumPlayers = 0;

    // Use this for initialization
    void Start () {
        int yPixel = 0;
        int xPixel = 0;
	    for(int columnIndex = 0; columnIndex < Columns; ++columnIndex)
        {
            for(int rowIndex = 0; rowIndex < Rows; ++rowIndex)
            {
                GameObject newTile = GameObject.Instantiate(tile);

                Vector3 position = new Vector3(xPixel, yPixel, 0.0f);

                newTile.transform.position = position;

                TileHandler handler = newTile.GetComponent<TileHandler>();
                if (handler)
                {
                    bool isStartTile = ((columnIndex == 1 && rowIndex == 1) || (columnIndex == Columns - 2 && rowIndex == Rows - 2));
                    bool isBlockingTile = ((columnIndex == 0 || rowIndex == 0) || (columnIndex == Columns - 1 || rowIndex == Rows - 1));

                    handler.SetCurrentType(isStartTile ? -1 : isBlockingTile ? -2 : 0);
                    if(handler.CurrentType == -1)
                    {
                        StartTiles.Add(newTile);
                    }
                    Tiles.Add(newTile);
                    handler.Index = Tiles.Count - 1;
                }

                xPixel += PixelSize;
            }

            xPixel = 0;
            yPixel += PixelSize;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Initialize(List<GameObject> players)
    {
        NumPlayers = players.Count;
        if(NumPlayers > StartTiles.Count)
        {
            Debug.LogError("Not enough start tiles for players!");
            return;
        }

        for(int index = 0; index < NumPlayers; ++index)
        {
            // Set position of player[index] to StartTiles[index]
            players[index].transform.position = StartTiles[index].transform.position;
        }

        ResetGrid();
    }

    public void ResetGrid()
    {
        for (int index = 0; index < Tiles.Count; ++index)
        {
            TileHandler handler = Tiles[index].GetComponent<TileHandler>();
            if (handler && handler.CurrentType > (int)TileHandler.TileType.Empty)
            {
                handler.SetCurrentType((int)TileHandler.TileType.Empty);
            }
        }
    }

    public GameObject GetTileGameObject(Vector3 position)
    {
        return GetTileGameObject(new Vector2(position.x, position.y));
    }

    public TileHandler GetTileScript(Vector3 position)
    {
        return GetTileScript(new Vector2(position.x, position.y));
    }

    public GameObject GetTileGameObject(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / Columns);
        int y = Mathf.FloorToInt(position.y / Rows);

        float percentileX = (float)x / (float)(PixelSize * Columns);
        float percentileY = (float)y / (float)(PixelSize * Columns);
        int index = Mathf.FloorToInt(percentileX * (float)Columns) + Mathf.FloorToInt((percentileY * (float)Rows) * Columns);
        //Debug.Log("[" + position.x.ToString() + "," + position.y.ToString() + "], index = " + index.ToString());
        return Tiles[index];
    }

    public TileHandler GetTileScript(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / Columns);
        int y = Mathf.FloorToInt(position.y / Rows);

        float percentileX = position.x / (float)(PixelSize * Columns);
        float percentileY = position.y / (float)(PixelSize * Columns);
        int index = Mathf.FloorToInt(percentileX * (float)Columns) + Mathf.FloorToInt((percentileY * (float)Rows) * Columns);
        //Debug.Log("[" + position.x.ToString() + "," + position.y.ToString() + "], index = " + index.ToString());
        return (Tiles[index]).GetComponent<TileHandler>();
    }

    public int GetTileIndex(Vector3 position)
    {
        return GetTileIndex(new Vector2(position.x, position.y));
    }

    public int GetTileIndex(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / Columns);
        int y = Mathf.FloorToInt(position.y / Rows);

        float percentileX = position.x / (float)(PixelSize * Columns);
        float percentileY = position.y / (float)(PixelSize * Columns);
        int index = Mathf.FloorToInt(percentileX * (float)Columns) + Mathf.FloorToInt((percentileY * (float)Rows) * Columns);
        //Debug.Log("[" + position.x.ToString() + "," + position.y.ToString() + "], index = " + index.ToString());
        return index;
    }

    public TileHandler GetTileScript(GameObject tile)
    {
        return tile.GetComponent<TileHandler>();
    }

    public GameObject GetTileGameObject(int x, int y)
    {
        int index = (x / (PixelSize * Columns) * Columns) + ((y / (PixelSize * Rows) * Rows) * Columns);
        return Tiles[index];
    }

    public TileHandler GetTileScript(int x, int y)
    {
        int index = (x / (PixelSize * Columns) * Columns) + ((y / (PixelSize * Rows) * Rows) * Columns);
        return (Tiles[index]).GetComponent<TileHandler>();
    }
}
