using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public class TileData{public bool Passable;}
    private TileData[,] m_MapData;
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();

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
               }
              
               m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
           }
       }
       public Vector3 TileToWorld(Vector2Int tileIndex){return m_Grid.GetCellCenterWorld((Vector3Int)tileIndex);}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
