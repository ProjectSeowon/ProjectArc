using Unity.VersionControl.Git.ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MapController m_Map;
    private Vector2Int m_TilePosition;

    private bool m_GameOver;

    public float MoveSpeed = 5.0f;

    private bool m_Moving;
    private Vector3 m_Target;

    public void Init()
    {
        m_Moving = false;
        m_GameOver = false;
    }

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
        Move(tile, true);
    }
    public void Move(Vector2Int tile, bool immediate)
    {
        //Debug.Log($"Moving player to tile: {tile}");
        
        m_TilePosition = tile;

        if (immediate)
        {
            m_Moving = false;
            transform.position = m_Map.TileToWorld(tile);
        }else{
            m_Moving = true;
            m_Target = m_Map.TileToWorld(m_TilePosition);
            //transform.position = m_Map.TileToWorld(tile);
        }
    }
    private void Start() {
        if (m_Map == null) {
            Debug.LogWarning("MapController reference is null. Ensure Spawn is called before Update.");
        }
    }

    public void GameOver(){m_GameOver = true;}

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("Update is running in Player");

        if (m_Map == null) return;  // Ensure map is assigned before doing anything

        Vector2Int newTileTarget = m_TilePosition;
        bool hasMoved = false;

        if (m_GameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame){GameManager.Instance.StartNewGame();}
            
            return;
        }

        if (Keyboard.current == null)
        {
            Debug.LogError("Keyboard input system is not initialized!");
            return;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            //Debug.Log("W key pressed");
            newTileTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            //Debug.Log("S key pressed");
            newTileTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            //Debug.Log("D key pressed");
            newTileTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            //Debug.Log("A key pressed");
            newTileTarget.x -= 1;
            hasMoved = true;
        }

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

        if (m_Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Target, MoveSpeed * Time.deltaTime);
            if (transform.position == m_Target)
            {
                m_Moving = false;
                var tileData = m_Map.GetTileData(m_TilePosition);
                if (tileData.ContainedObject != null){tileData.ContainedObject.PlayerWantsToEnter();}
            }
            return;
        }
        if(hasMoved)
        {
            MapController.TileData tileData = m_Map.GetTileData(newTileTarget);

            if(tileData != null && tileData.Passable)
            {
                GameManager.Instance.TurnManager.Tick();

                if (tileData.ContainedObject == null){Move(newTileTarget, false);}
                else if (tileData.ContainedObject.PlayerWantsToEnter())
                {
                    Move(newTileTarget, true);
                    tileData.ContainedObject.PlayerHereNow();
                }
            }
        }
        
    }

}
