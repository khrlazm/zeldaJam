using UnityEngine;


public class BreakableObject : MonoBehaviour
{
    public int hp = 1;
    public GameObject brokenPrefab; // optional debris
    public GameObject dropPrefab; // heart, key, etc.
    public float dropChance = 0.35f;


    public void Damage(int amount)
    {
        hp -= amount;
        if (hp <= 0) Break();
    }


    public void Break()
    {
        if (brokenPrefab != null)
        {
            Instantiate(brokenPrefab, transform.position, transform.rotation);
        }
        if (dropPrefab != null && Random.value < dropChance)
        {
            Instantiate(dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}