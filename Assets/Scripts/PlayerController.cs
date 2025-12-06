using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Combat Settings")]
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public LayerMask hittableLayers;

    [Header("Input Actions (New Input System)")]
    public InputActionReference moveAction;
    public InputActionReference attackAction;
    public InputActionReference interactAction;

    private Vector2 moveInput;
    private bool isAttacking = false;

    [Header("Combat Settings")]
    public int attackCount = 3;  // how many attack animations you have
    private int lastAttackIndex = 0;


    private void Awake()
    {
        // Auto-assign references if missing
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!animator) animator = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        attackAction.action.Enable();
        interactAction.action.Enable();

        attackAction.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= OnAttack;

        moveAction.action.Disable();
        attackAction.action.Disable();
        interactAction.action.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        animator.SetFloat("Speed", direction.magnitude);

        if (direction.sqrMagnitude > 0.001f && !isAttacking)
        {
            Vector3 targetPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.MoveRotation(
                Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime)
            );
        }
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (isAttacking) return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // pick random attack index
        int atkIndex = Random.Range(0, attackCount);
        animator.SetInteger("AttackIndex", atkIndex);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.15f);

        // Damage hitbox trigger
        Vector3 center = transform.position + transform.forward * attackRange * 0.6f;
        Collider[] hits = Physics.OverlapSphere(center, attackRange, hittableLayers);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Health health))
                health.TakeDamage(attackDamage);

            if (hit.TryGetComponent(out BreakableObject brk))
                brk.Break();
        }

        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * attackRange * 0.6f;
        Gizmos.DrawWireSphere(center, attackRange);
    }
}
