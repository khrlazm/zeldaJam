using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class KeyPickupCinematic : MonoBehaviour
{
    [Header("References")]
    public Transform keyHoldPoint;            // Where key hovers
    public float hoverHeight = 0.5f;
    public float rotateSpeed = 90f;

    [Header("Cinemachine")]
    public CinemachineCamera pickupCamera;    // cinematic camera
    public CinemachineCamera mainCamera;      // default gameplay camera

    [Header("Player")]
    public Animator playerAnimator;
    public string holdKeyTrigger = "HoldKey";

    public float cinematicDuration = 1f;

    private bool isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;

        if (other.TryGetComponent<PlayerController>(out var pc))
        {
            StartCoroutine(PickupRoutine(pc));
            isPickedUp = true;
        }
    }

    private IEnumerator PickupRoutine(PlayerController pc)
    {
        // Disable collider to prevent multiple pickups
        GetComponent<Collider>().enabled = false;

        // Pause player input
        pc.enabled = false;

        // Animator hold pose
        if (playerAnimator != null)
            playerAnimator.SetTrigger(holdKeyTrigger);

        // Switch to pickup camera
        if (pickupCamera != null) pickupCamera.Priority = 20;
        if (mainCamera != null) mainCamera.Priority = 0;

        // Turn player to face camera
        Vector3 lookDir = pickupCamera.transform.position - pc.transform.position;
        lookDir.y = 0;
        pc.transform.rotation = Quaternion.LookRotation(lookDir);

        // ENABLE HEAD LOOK-AT HERE
        var headLook = pc.GetComponent<HeadLookController>();
        if (headLook != null)
        {
            headLook.lookTarget = pickupCamera.transform;
            headLook.EnableLookAt(true);
        }

        // --- CINEMATIC LOOP ---
        float timer = 0f;
        while (timer < cinematicDuration)
        {
            timer += Time.deltaTime;

            // Hover key
            if (keyHoldPoint != null)
            {
                Vector3 targetPos = keyHoldPoint.position + Vector3.up * hoverHeight;
                transform.position = Vector3.Lerp(transform.position, targetPos, 10f * Time.deltaTime);
            }

            // Rotate key
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

            yield return null;
        }

        // DISABLE LOOK-AT AFTER CINEMATIC
        if (headLook != null)
            headLook.EnableLookAt(false);

        // Resume control
        pc.enabled = true;

        // Switch back camera
        if (pickupCamera != null) pickupCamera.Priority = 0;
        if (mainCamera != null) mainCamera.Priority = 20;

        // Remove key
        gameObject.SetActive(false);
    }



}
