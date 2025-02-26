using UnityEngine;

public class Player : MonoBehaviour
{
    private MapController m_Map;
    private Vector2Int m_TilePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Spawn(MapController map, Vector2Int tile)
    {
        m_Map = map;
        m_TilePosition = tile;

        transform.position = m_Map.TileToWorld(tile);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
