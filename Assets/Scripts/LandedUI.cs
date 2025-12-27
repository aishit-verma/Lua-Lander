using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI nextButtonText;
    [SerializeField] private Button nextButton;
    private Action nextButtonClickAction;
    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }
    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        
        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Successful)
        {
            titleText.text = "Successful Landing!";
            nextButtonText.text = "CONTINUE";
            nextButtonClickAction = GameManager.Instance.GoToNextLevel;
        }
        else
        {
            titleText.text = "<color=red>CRASH!</color>";
            nextButtonText.text = "RETRY";
            nextButtonClickAction = GameManager.Instance.RestartLevel;
        }
        statsText.text = Mathf.Round(e.landingSpeed) + "\n"+
        Mathf.Round(e.dotVector * 100f)+"\n"+
        "x"+e.scoreMultiplier+"\n"+
        e.score;
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
        nextButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

