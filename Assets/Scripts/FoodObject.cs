using UnityEngine;

public class FoodObject : TileObject
{
    public int AG = 10;
    public override void PlayerHereNow()
    {
        Destroy(gameObject);

        GameManager.Instance.FoodChanger(AG);
        
    }
    
}
