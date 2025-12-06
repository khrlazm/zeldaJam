using UnityEngine;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public int keys = 0;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    public void AddKey()
    {
        keys++;
    }


    public bool UseKey()
    {
        if (keys > 0)
        {
            keys--;
            return true;
        }
        return false;
    }
}