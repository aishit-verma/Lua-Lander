using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
