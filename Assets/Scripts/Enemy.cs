using Unity.VisualScripting;
using UnityEngine;
public class Enemy : TileObject
{
    public int HP = 3;
    private int m_CurrentHP;

    private void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord){
        base.Init(coord);
        m_CurrentHP = HP;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHP -= 1;
        if (m_CurrentHP <= 0){
            Destroy(gameObject);
        }
        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var map = GameManager.Instance.MapController;
        var targetTile = map.GetTileData(coord);
        
        if (targetTile == null || !targetTile.Passable || targetTile.ContainedObject != null)
        {
            return false;
        }

        var currentTile = map.GetTileData(m_Tile);
        currentTile.ContainedObject = null;

        targetTile.ContainedObject = this;
        m_Tile = coord;
        transform.position = map.TileToWorld(coord);

        return true;
    }

    void TurnHappened()
    {
        var playerTile = GameManager.Instance.Player.Tile();

        int xDist = playerTile.x - m_Tile.x;
        int yDist = playerTile.y - m_Tile.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1)) 
        {
            GameManager.Instance.FoodChanger(3);
        }else{
            if (absXDist > absYDist){
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }else{
               if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                } 
            }
        }
    }

    bool TryMoveInX(int xDist)
    {
        if (xDist > 0){return MoveTo(m_Tile + Vector2Int.right);}

        return MoveTo(m_Tile + Vector2Int.left);
    }

    bool TryMoveInY(int yDist)
    {
        if (yDist > 0){return MoveTo(m_Tile + Vector2Int.left);}

        return MoveTo(m_Tile + Vector2Int.right);
    }
}