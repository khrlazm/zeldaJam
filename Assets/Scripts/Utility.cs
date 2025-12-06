// ------------------------------
// File: Utility.cs (small helpers)
// ------------------------------
using System.Collections;
using UnityEngine;


public static class Utility
{
    public static IEnumerator FlashRenderersCoroutine(Renderer[] renderers, float duration)
    {
        float t = 0f;
        bool on = false;
        while (t < duration)
        {
            foreach (var r in renderers) if (r != null) r.enabled = on;
            on = !on;
            t += 0.06f;
            yield return new WaitForSeconds(0.06f);
        }
        foreach (var r in renderers) if (r != null) r.enabled = true;
    }


    public static IEnumerator InvincibleCoroutine(Health h, float duration)
    {
        if (h == null) yield break;
        h.isInvincible = true;
        yield return new WaitForSeconds(duration);
        h.isInvincible = false;
    }
}