using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    public float flashDuration = 0.15f;
    private Renderer[] renderers;
    private Color[] originalColors;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // turn white
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        // restore color
        for (int i = 0; i < renderers.Length; i++)
            if (renderers[i] != null)
                renderers[i].material.color = originalColors[i];
    }
}
