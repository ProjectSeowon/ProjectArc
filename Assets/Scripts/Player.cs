using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MapController m_Map;
    private Vector2Int m_TilePosition;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Spawn(MapController map, Vector2Int tile)
    {
       if (map == null)
        {
            Debug.LogError("Spawn() received a null MapController!");
            return;
        }

        Debug.Log("Assigning m_Map...");
        m_Map = map;

        Debug.Log("m_Map assigned successfully, calling Move...");
        Move(tile);
    }
    public void Move(Vector2Int tile){
        m_TilePosition = tile;

        transform.position = m_Map.TileToWorld(tile);
    }
    private void Start() {
    if (m_Map == null) {
        Debug.LogWarning("MapController reference is null. Ensure Spawn is called before Update.");
    }
}

    // Update is called once per frame
    private void Update()
    {
        if (m_Map == null) return;

        Vector2Int newTileTarget = m_TilePosition;
        bool hasMoved = false;

        if(Keyboard.current.wKey.wasPressedThisFrame){
            newTileTarget.y += 1;
            hasMoved = true;}
        else if(Keyboard.current.sKey.wasPressedThisFrame){
            newTileTarget.y -= 1;
            hasMoved = true;}
        else if(Keyboard.current.dKey.wasPressedThisFrame){
            newTileTarget.x += 1;
            hasMoved = true;}
        else if(Keyboard.current.aKey.wasPressedThisFrame){
            newTileTarget.x -= 1;
            hasMoved = true;}
        else if(Keyboard.current.upArrowKey.wasPressedThisFrame){
            newTileTarget.y += 1;
            hasMoved = true;}
        else if(Keyboard.current.downArrowKey.wasPressedThisFrame){
            newTileTarget.y -= 1;
            hasMoved = true;}
        else if(Keyboard.current.rightArrowKey.wasPressedThisFrame){
            newTileTarget.x += 1;
            hasMoved = true;}
        else if(Keyboard.current.leftArrowKey.wasPressedThisFrame){
            newTileTarget.x -= 1;
            hasMoved = true;}

        
        if(hasMoved){
            MapController.TileData tileData = m_Map.GetTileData(newTileTarget);

            if(tileData != null && tileData.Passable){
               Move(m_TilePosition);
            }
        }
        
    }

}
