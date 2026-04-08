using UnityEngine;

public class BloodParticleSystemHandler : MonoBehaviour
{
    public static BloodParticleSystemHandler Instance { get; private set; }

    [SerializeField] private Vector2 splatSizeRange = new Vector2(0.5f, 1.5f);
    [SerializeField] private string sortingLayerName = "Decals";
    [SerializeField] private int sortingOrder = 10;

    private MeshParticleSystem meshParticleSystem;

    private void Awake()
    {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();

        var mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sortingLayerName = sortingLayerName;
            mr.sortingOrder = sortingOrder;

            var spriteShader = Shader.Find("Sprites/Default");
            if (spriteShader != null && mr.material.shader != spriteShader)
                mr.material.shader = spriteShader;
            mr.material.renderQueue = 3000;
        }
    }

    public void SpawnBlood(Vector3 worldPosition)
    {
        Transform meshTransform = meshParticleSystem.transform;
        Vector3 localPosition = meshTransform.InverseTransformPoint(worldPosition);

        float size = Random.Range(splatSizeRange.x, splatSizeRange.y);
        Vector3 quadSize = new Vector3(size, size);
        float rotation = Random.Range(0f, 360f);

        int uvIndex = Random.Range(0, meshParticleSystem.GetUVCount());

        meshParticleSystem.AddQuad(localPosition, rotation, quadSize, false, uvIndex);
    }
}
