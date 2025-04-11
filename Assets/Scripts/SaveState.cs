using Unity.VisualScripting;
using UnityEngine;
public class SaveState : MonoBehaviour
{    
    int level;
    public void SaveGame()
    {
        GameManager GameManager = FindFirstObjectByType<GameManager>();
        SaveData SaveData = new SaveData();
        SaveData.Level = GameManager.GetLevel();
        SaveData.HP = GameManager.GetHP();
        SaveData.Food = GameManager.GetFood();
        string json = JsonUtility.ToJson(SaveData);
        string path = Application.persistentDataPath + "/save.json";
        System.IO.File.WriteAllText(path, json);
    }
    
    public void LoadGame()
    {
        GameManager GameManager = FindFirstObjectByType<GameManager>();
        string path = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(path))
        {
            level = GameManager.GetLevel();
            string json = System.IO.File.ReadAllText(path);
            SaveData LoadedData = JsonUtility.FromJson<SaveData>(json);
            if( level != LoadedData.Level)
            {
                GameManager.LoadLevel(LoadedData.Level);
                GameManager.LoadHP(LoadedData.HP);
                GameManager.LoadFood(LoadedData.Food);
            }
        }else{
            Debug.LogWarning("No save file found!");
        }
    }
}