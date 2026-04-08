using UnityEngine;
using TMPro; // <-- TMP namespace

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private TMP_Text ammoText; // <-- TMP text type
    [SerializeField] private AudioClip emptyClickSfx;
    [SerializeField][Range(0f, 1f)] private float emptyClickVolume = 1f;

    private int currentAmmo;

    void OnEnable()
    {
        ParryController.OnParrySuccess += Reload;
    }

    void OnDisable()
    {
        ParryController.OnParrySuccess -= Reload;
    }

    void Awake()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
    }

    public bool TryConsumeBullet()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateUI();
            return true;
        }
        else
        {
            // play empty click sound
            if (emptyClickSfx != null)
                AudioSource.PlayClipAtPoint(emptyClickSfx, Camera.main.transform.position, emptyClickVolume);
            return false;
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
        // optional: play reload sound/flash here
    }

    private void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}
