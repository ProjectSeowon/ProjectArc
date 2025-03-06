using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    public MapController MapController;
    public Player Player;

    public TurnManager TurnManager {get; private set;}

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnManager = new TurnManager();
        
        MapController.Init();
        Player.Spawn(MapController, new Vector2Int(1, 1));
        MapController.player = Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
