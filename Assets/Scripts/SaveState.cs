using Unity.VisualScripting;
using UnityEngine;
public class SaveState : MonoBehaviour
{
    public GameManager GameManager;
    public MapController Map;
    private int level;
    private int playerHP = 100;
    private int playerFood = 100;
    private string Save;
    private Save SV()
    void Start()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        Map = FindFirstObjectByType<MapController>();
    }
    
    public void SaveGame()
    {
        
        Save = JsonUtility.ToJson(SV());
    }
    
    public void LoadGame()
    {
        JsonUtility.FromJson(Save, SV());
    }

    public int GetHP()
    {
        return 0;
    }

    public int GetFood()
    {
        return 0;
    }

    public int GetLevel()
    {
        return -1;
    }

}