using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public string mainSceneName = "GameScene";


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    public void OnPlayerDeath()
    {
        UIManager.Instance.ShowGameOver();
    }


    public void OnRelicPicked()
    {
        UIManager.Instance.ShowVictory();
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(mainSceneName);
    }


    public void QuitToDesktop()
    {
        Application.Quit();
    }
}