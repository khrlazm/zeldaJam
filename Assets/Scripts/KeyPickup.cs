using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class KeyPickup : MonoBehaviour
{
    public Transform keyHoldPoint;
    public float hoverHeight = 0.5f;
    public float rotateSpeed = 90f;
    public float pickupDuration = 2f;

    public CinemachineCamera pickupCamera;       // ? updated
    public Animator playerAnimator;
    public string holdKeyTrigger = "HoldKey";

    private bool isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;
        var pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            StartCoroutine(PickupRoutine(pc.transform));
            isPickedUp = true;
        }
    }

    private IEnumerator PickupRoutine(Transform player)
    {
        GetComponent<Collider>().enabled = false;

        if (playerAnimator != null)
            playerAnimator.SetTrigger(holdKeyTrigger);

        if (pickupCamera != null)
            pickupCamera.Priority = 20;

        float timer = 0f;

        while (timer < pickupDuration)
        {
            timer += Time.deltaTime;

            if (keyHoldPoint != null)
            {
                Vector3 targetPos = keyHoldPoint.position + Vector3.up * hoverHeight;
                transform.position = Vector3.Lerp(transform.position, targetPos, 10f * Time.deltaTime);
            }

            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

            yield return null;
        }

        if (pickupCamera != null)
            pickupCamera.Priority = 0;

        gameObject.SetActive(false);
    }
}
