using UnityEngine;

public class HeadLookController : MonoBehaviour
{
    public Animator animator;
    public Transform lookTarget;

    [Range(0, 1)] public float lookWeight = 1f;
    [Range(0, 1)] public float bodyWeight = 0.1f;
    [Range(0, 1)] public float headWeight = 1f;
    [Range(0, 1)] public float eyesWeight = 1f;
    [Range(0, 1)] public float clampWeight = 0.5f;

    private bool enableLook = false;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    public void EnableLookAt(bool state)
    {
        enableLook = state;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!enableLook || lookTarget == null)
        {
            animator.SetLookAtWeight(0f);
            return;
        }

        animator.SetLookAtWeight(
            lookWeight,
            bodyWeight,
            headWeight,
            eyesWeight,
            clampWeight
        );

        animator.SetLookAtPosition(lookTarget.position);
    }
}
