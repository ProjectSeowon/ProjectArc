using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public SaveState Save;
    
    public MapController MapController;
    public Player Player;
    private int m_Food;
    private int m_CurLevel = 1;
    private int m_HP = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOver;
    private Label m_GameOverMessage;
    private Label m_HPLabel;
    private VisualElement m_StartGame;
    private Label m_StartGameMessage;
    private Label m_Title;

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

        m_HPLabel = UIDoc.rootVisualElement.Q<Label>("HPLabel");
        m_HPLabel.text = "HP: " + m_HP / 2;
        m_StartGame = UIDoc.rootVisualElement.Q<VisualElement>("StartGame");
        m_StartGame.style.visibility = Visibility.Visible;
        m_StartGameMessage = UIDoc.rootVisualElement.Q<Label>("StartGameMessage");
        m_Title = UIDoc.rootVisualElement.Q<Label>("Title");
        m_Title.text = "ProjectArc";
        m_StartGameMessage.text = "Press Space to start game";

        StartNewGame();
    }

    public void HideStartGame()
    {m_StartGame.style.visibility = Visibility.Hidden;}

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

    public int GetLevel()
    {
        return m_CurLevel;
    }
    
    void OnTurn()
    {
        FoodChanger(-1);
    }
    
    public void FoodChanger(int a)
    {
        Debug.Log(m_Food);
        if (m_Food > 0){
            m_Food += a;
            m_FoodLabel.text = "Food: " + m_Food / 2;
        }else if(m_HP <= 0){
            Player.GameOver();
            m_GameOver.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurLevel + " levels";
        }else if (m_Food == 1 || m_Food == 0){
            if (a > 0){
                m_Food += a;
                m_FoodLabel.text = "Food: " + m_Food / 2;
            }else{
                m_Food -= 1;
                m_HP -= 1;
                m_HPLabel.text = "HP: " + m_HP / 2;
            }
            
        }else if (m_Food < 0){
            if (m_HP >= 200){
                m_Food += a + 1;
                m_FoodLabel.text = "Food: " + m_Food / 2;
            }else{
                m_HP += a;
                m_HPLabel.text = "HP: " + m_HP / 2; 
            } 
        }
    }

    public int GetFood(){return m_Food;}

    public int GetHP(){return m_HP;}

    public void NewLevel()
    {
        MapController.Clean();
        MapController.Init();
        Player.Spawn(MapController, new Vector2Int(1,1));
        MapController.player = Player;

        m_CurLevel++;
    }

    void Update()
    {
        if (Keyboard.current.altKey.wasPressedThisFrame){Save.SaveGame();}
        else if (Keyboard.current.xKey.wasPressedThisFrame && !Player.GameStart())
        {
            Save.LoadGame();
            m_CurLevel = Save.GetLevel();
            m_Food = Save.GetFood();
            m_HP = Save.GetHP();
            m_HPLabel.text = "HP: " + m_HP / 2;
            m_FoodLabel.text = "Food: " + m_Food / 2;
        }
    }
}
