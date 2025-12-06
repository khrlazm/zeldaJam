using UnityEngine;


public class Door : MonoBehaviour
{
    public bool requiresKey = true;
    public Animator animator;
    public Collider doorCollider;


    bool isOpen = false;


    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (doorCollider == null) doorCollider = GetComponent<Collider>();
    }


    public void TryOpen()
    {
        if (isOpen) return;
        if (requiresKey)
        {
            if (Inventory.Instance.UseKey()) Open();
            else UIManager.Instance.ShowPopup("You need a key.");
        }
        else Open();
    }


    public void Open()
    {
        isOpen = true;
        animator?.SetTrigger("Open");
        if (doorCollider != null) doorCollider.enabled = false;
    }


    public void ForceOpen()
    {
        isOpen = true;
        animator?.SetTrigger("Open");
        if (doorCollider != null) doorCollider.enabled = false;
    }
}