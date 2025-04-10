public class Save
{
    public GameManager GameManager;
    public MapController Map;
    public int Food;
    public int HP;
    public int Level;

    public Init()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        Map = FindFirstObjectByType<MapController>();
        Level = GameManager.GetLevel();
        HP = GameManager.GetHP();
        Food = GameManager.GetFood();
    }
}