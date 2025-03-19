using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : TileObject
{
    public Tile ObstacleTile;
    public int MaxHealth = 3;

    private int m_HP;
    private Tile m_OGTile;

    public override void Init(Vector2Int tile)
    {
        base.Init(tile);

        m_HP = MaxHealth;

        m_OGTile = GameManager.Instance.MapController.GetTile(tile);
        GameManager.Instance.MapController.SetTile(tile, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        m_HP -= 1;

        if (m_HP > 0){return false;}
        
        GameManager.Instance.MapController.SetTile(m_Tile, m_OGTile);
        Destroy(gameObject);
        return true;
    }
}
