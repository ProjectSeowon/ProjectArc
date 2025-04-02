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
    void Start()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        Map = FindFirstObjectByType<MapController>();
    }
    
    public void SaveGame()
    {
        level = GameManager.GetLevel();
        playerHP = GameManager.GetHP();
        playerFood = GameManager.GetFood();
        Save = JsonUtility.ToJson(this);
    }
    
    public void LoadGame()
    {
        JsonUtility.FromJsonOverwrite(Save, this);
    }

    public int GetHP()
    {
        return playerHP;
    }

    public int GetFood()
    {
        return playerFood;
    }

    public int GetLevel()
    {
        return level;
    }

}