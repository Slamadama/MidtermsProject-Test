using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFix : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black; // or any color you want
    }

    void OnPreRender()
    {
        // Now 'cam' exists because we cached it in Awake
        GL.Clear(true, true, cam.backgroundColor);
    }
}
