using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 2f;
    public float agroRange = 8f;
    public float attackRange = 1.2f;
    public int damage = 1;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    [Header("Optional (for animations)")]
    public Animator animator;

    private Rigidbody rb;
    private Transform player;
    private float lastAttack = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        var p = FindFirstObjectByType<PlayerController>();
        if (p != null)
            player = p.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > agroRange)
        {
            // Outside range — idle state
            if (animator) animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        dir.Normalize();

        // In agro range — face player
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(rot);
        }

        if (dist > attackRange)
        {
            // Chase player
            Vector3 targetPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            if (animator) animator.SetFloat("Speed", 1f);
        }
        else
        {
            // Attack player
            if (Time.time - lastAttack >= attackCooldown)
            {
                lastAttack = Time.time;
                TryDealDamage();

                if (animator)
                {
                    animator.SetTrigger("Attack");
                    animator.SetFloat("Speed", 0f);
                }
            }
        }
    }

    private void TryDealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var h))
            {
                h.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
