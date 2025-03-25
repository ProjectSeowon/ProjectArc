using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    public MapController MapController;
    public Player Player;
    private int m_Food = 200;
    private int m_CurLevel = 1;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOver;
    private Label m_GameOverMessage;

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

        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the Inspector!");
            return;
        }
        
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food: " + m_Food / 2;
        m_GameOver = UIDoc.rootVisualElement.Q<VisualElement>("GameOver");
        m_GameOverMessage = m_GameOver.Q<Label>("GameOverMessage");

        StartNewGame();
    }

    public void StartNewGame()
    {
        MapController.Clean();
        MapController.Init();

        m_GameOver.style.visibility = Visibility.Hidden;

        m_CurLevel = 1;
        m_Food = 100;
        m_FoodLabel.text = "Food: " + m_Food / 2;

        
        Player.Init();
        Player.Spawn(MapController, new Vector2Int(1, 1));
    }
    
    void OnTurn()
    {
        FoodChanger(-1);
    }
    
    public void FoodChanger(int a)
    {
        m_Food += a;
        m_FoodLabel.text = "Food: " + m_Food / 2;

        if (m_Food <= 0)
        {
            Player.GameOver();
            m_GameOver.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurLevel + " levels";
        }
    }

    public void NewLevel()
    {
        MapController.Clean();
        MapController.Init();
        Player.Spawn(MapController, new Vector2Int(1,1));
        MapController.player = Player;

        m_CurLevel++;
    }
}
