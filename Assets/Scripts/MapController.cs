using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public FoodObject FoodPrefab;
    public SmallFoodObject SmallFoodPrefab;
    public WallObject WallPrefab;
    public ExitTileObject ExitTilePrefab;
    
    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public class TileData
    {
        public bool Passable; 
        public TileObject ContainedObject;
    }
    private TileData[,] m_MapData;
    public Player player;
    private List<Vector2Int> m_EmptyTileList;
    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        if (m_Tilemap == null || m_Grid == null)
        {
            Debug.LogError("Tilemap or Grid is not assigned in MapController!");
            return;
        }

        Debug.Log("Initializing MapController...");
        
        
        if (player == null)
        {
            Debug.LogError("Player reference is not set in the Inspector!");
            return;
        }

        Debug.Log("Player reference found, calling Spawn...");
        m_EmptyTileList = new List<Vector2Int>();
        m_MapData = new TileData[Width, Height];

        
       for (int y = 0; y < Height; ++y)
       {
           for(int x = 0; x < Width; ++x)
           {
               Tile tile;
              
                m_MapData[x, y] = new TileData();

               if(x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
               {
                   tile = WallTiles[Random.Range(0, WallTiles.Length)];
                   m_MapData[x, y].Passable = false;
               }
               else
               {
                   tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                   m_MapData[x, y].Passable = true;

                   m_EmptyTileList.Add(new Vector2Int(x, y));
               }
              
               m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
           }
       }
       m_EmptyTileList.Remove(new Vector2Int(1, 1));
       Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
       AddObject(Instantiate(ExitTilePrefab), endCoord);
       m_EmptyTileList.Remove(endCoord);

       GenWall();
       GenerateFood();
    }

    void AddObject(TileObject obj, Vector2Int coord)
    {
        TileData data = m_MapData[coord.x, coord.y];
        obj.transform.position = TileToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    public Vector3 TileToWorld(Vector2Int tileIndex){
        return m_Grid.GetCellCenterWorld((Vector3Int)tileIndex);
    }

    public TileData GetTileData(Vector2Int tileIndex){
        if(tileIndex.x < 0 || tileIndex.x >= Width || tileIndex.y < 0 || tileIndex.y >= Height){
            return null;
        }

        return m_MapData[tileIndex.x, tileIndex.y];
    }

    void GenerateFood()
    {
        int FoodCount = Random.Range(1, 5);
        for(int i = 0; i < FoodCount; ++i)
        {
            int rand = Random.Range(0, 10);
            int randIDX = Random.Range(0, m_EmptyTileList.Count);
            Vector2Int coord = m_EmptyTileList[randIDX];
            if(rand % 2 == 0)
            {
                FoodObject newFood = Instantiate(FoodPrefab);
                AddObject(newFood, coord);
            }else{
                SmallFoodObject newSmallFood = Instantiate(SmallFoodPrefab);
                AddObject(newSmallFood, coord);
            }
            m_EmptyTileList.Remove(coord);
        }
    }

    void GenWall()
    {
        int WallCount = Random.Range(6, 10);
        for (int i = 0; i < WallCount; ++ i)
        {
            int randIDX = Random.Range(0, m_EmptyTileList.Count);
            Vector2Int coord = m_EmptyTileList[randIDX];

            TileData data = m_MapData[coord.x, coord.y];
            m_MapData[coord.x, coord.y].Passable = false;
            WallObject newWall = Instantiate(WallPrefab);

            AddObject(newWall, coord);
            m_EmptyTileList.Remove(coord);
        }
    }
    
    public void SetTile(Vector2Int tileIDX, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(tileIDX.x, tileIDX.y, 0), tile);
    }

    public Tile GetTile(Vector2Int tileIDX)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(tileIDX.x, tileIDX.y));
    }

    public void Clean()
    {
        if (m_MapData == null)
            return;
        
        for (int y = 0; y < Height; ++y)
        {
            for (int i = 0; i < Width; ++i)
            {
                var tileData = m_MapData[i ,y];
                if (tileData.ContainedObject != null)
                {
                    Destroy(tileData.ContainedObject.gameObject);
                }

                SetTile(new Vector2Int(i, y), null);
            }
        }
    }
}