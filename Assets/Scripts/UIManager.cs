using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    public void ShowPopup(string text)
    {
        Debug.Log("POPUP: " + text);
    }


    public void ShowGameOver()
    {
        Debug.Log("GAME OVER");
    }


    public void ShowVictory()
    {
        Debug.Log("VICTORY");
    }
}