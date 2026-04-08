using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CriticalStrikeFX : MonoBehaviour
{
    public static CriticalStrikeFX Instance { get; private set; }

    [Header("Screen Flash Settings")]
    [SerializeField] private Volume postProcessVolume;
    private ColorAdjustments colorAdjust;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float flashIntensity = 2f;
    [SerializeField] private Color firstFlashColor = Color.red;
    [SerializeField] private Color secondFlashColor = Color.white;

    [Header("Particles")]
    [SerializeField] private ParticleSystem critParticlePrefab;
    [SerializeField] private ParticleSystem lightRayPrefab;

    [Header("Camera Shake")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip critSfx;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float sfxStartTime = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (postProcessVolume != null)
            postProcessVolume.profile.TryGet(out colorAdjust);
    }

    public void PlayCriticalFX(Vector3 hitPoint)
    {
        if (lightRayPrefab != null)
        {
            ParticleSystem ps = Instantiate(lightRayPrefab, hitPoint, Quaternion.identity);
            ps.Play();

            float delay = ps.main.duration + ps.main.startLifetime.constantMax;
            StartCoroutine(DelayedCriticalFX(hitPoint, delay));
        }
        else
        {
            StartCoroutine(DelayedCriticalFX(hitPoint, 0f));
        }

        // Play SFX via AudioManager (with pitch randomization)
        if (critSfx != null)
        {
            Debug.Log("[CriticalStrikeFX] Playing crit SFX: " + critSfx.name);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySfx(critSfx, sfxVolume);
            else
                AudioSource.PlayClipAtPoint(critSfx, Camera.main.transform.position, sfxVolume);
        }
        else
        {
            Debug.LogWarning("[CriticalStrikeFX] critSfx is NULL!");
        }
    }

    private IEnumerator DelayedCriticalFX(Vector3 hitPoint, float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(FlashSequence());

        if (critParticlePrefab != null)
        {
            ParticleSystem crit = Instantiate(critParticlePrefab, hitPoint, Quaternion.identity);
            crit.Play();
            Destroy(crit.gameObject, crit.main.duration + crit.main.startLifetime.constantMax);
        }

        if (mainCam != null)
            StartCoroutine(CameraShake());
    }

    private IEnumerator FlashSequence()
    {
        yield return StartCoroutine(FlashRoutine(firstFlashColor));
        yield return StartCoroutine(FlashRoutine(secondFlashColor));
    }

    private IEnumerator FlashRoutine(Color flashColor)
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / flashDuration;

            if (colorAdjust != null)
                colorAdjust.postExposure.value = Mathf.Lerp(flashIntensity, 0f, p);

            yield return null;
        }

        if (colorAdjust != null)
            colorAdjust.postExposure.value = 0f;
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalPos = mainCam.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCam.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCam.transform.localPosition = originalPos;
    }
}
