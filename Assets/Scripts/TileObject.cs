using UnityEngine;

public class TileObject : MonoBehaviour
{
    protected Vector2Int m_Tile;

    public virtual void Init(Vector2Int tile)
    {
        m_Tile = tile;
    }

    public virtual void PlayerHereNow()
    {
        
    }
    public virtual bool PlayerWantsToEnter()
    {
    return true;
    }
}
