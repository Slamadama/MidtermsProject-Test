using UnityEngine;
using DG.Tweening;

public class PlayerSqueeze : MonoBehaviour
{
    private Vector3 originalScale;

    [Header("DOTween Squeeze (Y axis)")]
    public float squeezeY = 0.9f;        // compress vertically
    public float squeezeDuration = 0.06f;
    public float releaseDuration = 0.12f;
    public Ease squeezeEase = Ease.OutQuad;
    public Ease releaseEase = Ease.OutQuad;

    void Awake()
    {
        originalScale = transform.localScale;
        Debug.Log("[PlayerSqueeze] Awake: originalScale=" + originalScale);
    }

    public void TriggerSqueezeDOT()
    {
        Debug.Log("[PlayerSqueeze] TriggerSqueezeDOT called!");

        transform.DOKill(true);
        transform.localScale = originalScale;

        // Target scales: squash Y, stretch X
        float targetY = originalScale.y * squeezeY;
        float targetX = originalScale.x * (1f / squeezeY); // inverse to preserve volume

        Debug.Log($"[PlayerSqueeze] Target scales: X={targetX}, Y={targetY}");

        // Animate squash/stretch
        transform.DOScale(new Vector3(targetX, targetY, originalScale.z), squeezeDuration)
            .SetEase(squeezeEase)
            .OnComplete(() =>
            {
                Debug.Log("[PlayerSqueeze] Squeeze complete, releasing back to original");
                transform.DOScale(originalScale, releaseDuration).SetEase(releaseEase)
                    .OnComplete(() =>
                    {
                        Debug.Log("[PlayerSqueeze] Release complete, scale=" + transform.localScale);
                    });
            });
    }

}
