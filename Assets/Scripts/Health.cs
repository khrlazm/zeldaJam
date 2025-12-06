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

    private Renderer[] cachedRenderers;
    private Color[] originalColors;

    private void Awake()
    {
        currentHP = maxHP;
        player = GetComponent<PlayerController>();

        // Cache renderers for flashing (player only)
        cachedRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[cachedRenderers.Length];

        for (int i = 0; i < cachedRenderers.Length; i++)
            originalColors[i] = cachedRenderers[i].material.color;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        onHurt?.Invoke();

        // Flash enemy or player depending on component
        var enemyFlash = GetComponent<EnemyFlash>();
        if (enemyFlash != null)
        {
            enemyFlash.Flash();
        }
        else
        {
            StartCoroutine(PlayerFlash());
        }

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine(invincibleDuration));
        }
    }

    private IEnumerator PlayerFlash()
    {
        // Turn white
        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedRenderers[i].material.color = Color.white;

        yield return new WaitForSeconds(0.15f);

        // Restore
        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedRenderers[i].material.color = originalColors[i];
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private void Die()
    {
        onDie?.Invoke();

        GameManager gm = FindFirstObjectByType<GameManager>();

        if (gm != null && player != null)
        {
            gm.OnPlayerDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
