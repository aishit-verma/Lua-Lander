using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    public static PausedUI instance{ get; private set; }
    private void Awake()
    {
        instance = this;
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnpauseGame();
            Hide();
            
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.ResetGame();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        Hide();
        resumeButton.Select();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
