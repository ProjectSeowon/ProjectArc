using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    public MapController MapController;
    public Player Player;
    private int m_Food = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;

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
        TurnManager.OnTick += OnTurn;
        MapController = FindFirstObjectByType<MapController>();
        Player = FindFirstObjectByType<Player>();
        if (MapController == null)
        {
            Debug.LogError("MapController is not assigned in the Inspector!");
            return;
        }

        Debug.Log("MapController is assigned. Proceeding to Init...");
        MapController.Init();

        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the Inspector!");
            return;
        }

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food: " + m_Food;

        Player.Spawn(MapController, new Vector2Int(1, 1));
        MapController.player = Player;
    }
    void OnTurn()
    {
        m_Food -= 1;
        m_FoodLabel.text = "Food: " + m_Food;
        Debug.Log("Current amount of food : " + m_Food);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
