using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI statsTMP;
    [SerializeField]private GameObject UpArrow;
    [SerializeField]private GameObject DownArrow;
    [SerializeField]private GameObject LeftArrow;
    [SerializeField]private GameObject RightArrow;
    [SerializeField]private Image fuelImage;
    private void UpdateStats()
    {
        UpArrow.SetActive(Lander.Instance.GetSpeedY() >= 0);
        DownArrow.SetActive(Lander.Instance.GetSpeedY() < 0);
        LeftArrow.SetActive(Lander.Instance.GetSpeedX() < 0);
        RightArrow.SetActive(Lander.Instance.GetSpeedX() >= 0);


        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();


        
        statsTMP.text = GameManager.Instance.GetCurrentLevelNumber() + "\n" +
        GameManager.Instance.GetScore() + "\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX()*10f)) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY()*10f));
    }
    private void Update()
    {
        UpdateStats();
    }
}
