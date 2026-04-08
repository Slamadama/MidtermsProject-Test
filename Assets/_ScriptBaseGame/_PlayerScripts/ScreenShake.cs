using UnityEngine;
using System.Collections;

public class BulletRecoilShake : MonoBehaviour
{
    private Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake(float duration, float magnitude, Vector2 shotDir)
    {
        // Opposite of shot direction
        Vector3 offset = new Vector3(-shotDir.x, -shotDir.y, 0) * magnitude;
        StartCoroutine(DoDirectionalShake(duration, offset));
    }

    private IEnumerator DoDirectionalShake(float duration, Vector3 offset)
    {
        // First nudge
        transform.localPosition = originalPos + offset;
        yield return new WaitForSeconds(duration);

        // Return to center
        transform.localPosition = originalPos;
        yield return new WaitForSeconds(duration);

        // Second nudge
        transform.localPosition = originalPos + offset;
        yield return new WaitForSeconds(duration);

        // Back to center
        transform.localPosition = originalPos;
    }
}
