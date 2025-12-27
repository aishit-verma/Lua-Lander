using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public enum Scene
    {
        GameScene,
        MainMenuScene,
        GameOverScene,
    }
    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
