using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 3;
    public int currentHP;
    public bool isInvincible = false;
    public float invincibleDuration = 0.8f;

    [Header("Events")]
    public UnityEvent onHurt;
    public UnityEvent onDie;

    private PlayerController player;

    private void Awake()
    {
        currentHP = maxHP;
        player = GetComponent<PlayerController>();
    }

    public void FlashRenderers(float duration)
    {
        StartCoroutine(FlashRoutine(duration));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        // Get all renderers (SkinnedMeshRenderer, MeshRenderer)
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        // Create temporary white flash materials
        Material[] originalMats = new Material[rends.Length];

        for (int i = 0; i < rends.Length; i++)
        {
            originalMats[i] = rends[i].material;
            rends[i].material.color = Color.white;
        }

        yield return new WaitForSeconds(duration);

        // Restore original materials
        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != null)
                rends[i].material = originalMats[i];
        }
    }


    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        onHurt?.Invoke();

        // Flash player if they have a renderer flash method
        if (player != null)
            FlashRenderers(invincibleDuration);

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            // Start invincibility frames
            StartCoroutine(InvincibilityRoutine(invincibleDuration));
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    private void Die()
    {
        onDie?.Invoke();

        GameManager gm = FindFirstObjectByType<GameManager>();

        // If this is the player
        if (gm != null && player != null)
        {
            gm.OnPlayerDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }


}
