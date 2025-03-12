using UnityEngine;

public class FoodObject : TileObject
{
    public override void PlayerHereNow()
    {
        Destroy(gameObject);

        Debug.Log("Food increased");
    }
    
}
