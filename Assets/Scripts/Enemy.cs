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
        
    }
}