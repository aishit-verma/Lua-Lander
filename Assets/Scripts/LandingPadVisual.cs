using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
   [SerializeField] private TextMeshPro scoreMultiplierText;
    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierText.text = "x" +landingPad.GetScoreMultiplier();
    }
}
