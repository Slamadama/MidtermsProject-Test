using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float defaultDuration = 111f;
    [SerializeField] private float defaultMagnitude = 111f;

    private float duration;
    private float magnitude;

    private float shaketime = 0f;
    private Vector3 lastOffset = Vector3.zero;
    private Vector3 directionalOffset = Vector3.zero;

    [SerializeField] private float springFrequency = 111f;
    [SerializeField] private float springDamping = 111f;
    [SerializeField] private float forcedividend = 1f;

    private void LateUpdate()
    {
        // Reset last frame offset
        transform.localPosition -= lastOffset;
        lastOffset = Vector3.zero;

        if (shaketime > 0f && duration > 0f)
        {
            float normalizedTime = 1f - (shaketime / duration);

            // Cosine recoil curve
            float spring = Mathf.Cos(normalizedTime * Mathf.PI);

            Debug.Log($"[ScreenShake] shaketime={shaketime:F3}, duration={duration:F3}, normalizedTime={normalizedTime:F3}, spring={spring:F3}");

            Vector3 offset3;
            if (directionalOffset != Vector3.zero)
            {
                offset3 = directionalOffset * magnitude * spring;
                Debug.Log($"[ScreenShake] Directional offset applied: {offset3}");
            }
            else
            {
                Vector2 shakeOffset = Random.insideUnitCircle * magnitude * spring;
                offset3 = new Vector3(shakeOffset.x, shakeOffset.y, 0f);
                Debug.Log($"[ScreenShake] Random offset applied: {offset3}");
            }

            transform.localPosition += offset3;
            lastOffset = offset3;

            shaketime -= Time.deltaTime;
        }
        else if (shaketime > 0f && duration <= 0f)
        {
            Debug.LogWarning("[ScreenShake] Duration is zero or negative — skipping shake to avoid NaN.");
            shaketime = 0f;
        }
    }

    public void triggershake(float durationOverride = -1f, float magnitudeOverride = -1f)
    {
        duration = durationOverride > 0f ? durationOverride : defaultDuration;
        magnitude = magnitudeOverride > 0f ? magnitudeOverride : defaultMagnitude;
        shaketime = duration;

        // Only reset if you explicitly want random shake
        directionalOffset = Vector3.zero;

        Debug.Log($"[ScreenShake] Random shake triggered: duration={duration}, magnitude={magnitude}");
    }

    public void triggershake(float durationOverride, float magnitudeOverride, Vector2 shotDir, float shootForce)
    {
        duration = durationOverride > 0f ? durationOverride : defaultDuration;

        // Scale magnitude by shootForce
        float forceScale = shootForce / forcedividend; // adjust divisor to taste
        magnitude = (magnitudeOverride > 0f ? magnitudeOverride : defaultMagnitude) * forceScale;

        shaketime = duration;

        // Opposite of shot direction
        directionalOffset = new Vector3(-shotDir.x, -shotDir.y, 0f);

        Debug.Log($"[ScreenShake] Directional shake triggered: duration={duration}, magnitude={magnitude}, shotDir={shotDir}, shootForce={shootForce}, directionalOffset={directionalOffset}");
    }
}
