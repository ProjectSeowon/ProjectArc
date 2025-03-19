using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitTileObject : TileObject
{
    public Tile EndTile;

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        GameManager.Instance.MapController.SetTile(coord, EndTile);
    }

    public override void PlayerHereNow()
    {
        GameManager.Instance.NewLevel();
    }
}