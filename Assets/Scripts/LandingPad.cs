using UnityEngine;

public class LandingPad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int scoreMultiplier;
    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
