using UnityEngine;

public class StopMotionAnimator : MonoBehaviour
{
    [Tooltip("The desired frame rate for the stop-motion effect.")]
    [Range(1, 60)]
    public int stopMotionFPS = 12;

    private Animator animator;
    private float frameTime;
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("StopMotionAnimator requires an Animator component on the same GameObject.");
            enabled = false; // Disable the script if no Animator is found
            return;
        }

        // Calculate the time per frame based on the desired FPS
        frameTime = 1f / stopMotionFPS;
        timer = 0f;
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // If enough time has passed to advance a "frame"
        if (timer >= frameTime)
        {
            // Advance the animator by one frame time, effectively stepping the animation
            animator.speed = 1f; // Ensure animator is running normally for the step
            animator.Update(frameTime);
            animator.speed = 0f; // Pause the animator after the step

            // Reset the timer
            timer -= frameTime;
        }
    }
}