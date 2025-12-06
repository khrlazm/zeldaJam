using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float agroRange = 8f;
    public float attackRange = 1.2f;
    public int damage = 1;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    Rigidbody rb;
    Transform player;
    float lastAttack = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        var p = FindFirstObjectByType<PlayerController>();
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= agroRange)
        {
            Vector3 dir = (player.position - transform.position);
            dir.y = 0f;
            dir.Normalize();
            if (dist > attackRange)
            {
                Vector3 target = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(target);
                // face
                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
                    rb.MoveRotation(rot);
                }
            }
            else
            {
                if (Time.time - lastAttack > attackCooldown)
                {
                    TryDealDamage();
                    lastAttack = Time.time;
                }
            }
        }
    }

    void TryDealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        foreach (var hit in hits)
        {
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}