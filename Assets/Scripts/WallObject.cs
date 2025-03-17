using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : TileObject
{
    public Tile ObstacleTile;

    public override void Init(Vector2Int tile)
    {
        base.Init(tile);
        GameManager.Instance.MapController.SetTile(tile, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        return false;
    }
}
