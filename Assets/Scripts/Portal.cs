using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal;

    public Transform normalVisible;
    public Transform normalInvisible;

    public Camera portalCamera;
    public Renderer viewThroughRenderer;
    private RenderTexture viewThroughRenderTexture;
    private Material viewThroughMaterial;

    public Camera mainCamera;

    private Vector4 vectorPlane;

    void Start()
    {
        viewThroughRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        viewThroughRenderTexture.Create();

        viewThroughMaterial = viewThroughRenderer.material;
        viewThroughMaterial.mainTexture = viewThroughRenderTexture;

        portalCamera.targetTexture = viewThroughRenderTexture;

        var plane = new Plane(normalVisible.forward, transform.position);
        vectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
    }

    void LateUpdate()
    {
        var virtualPosition = TransformPositionBetweenPortals(this, targetPortal, mainCamera.transform.position);
        var virtualRotation = TransformRotationBetweenPortals(this, targetPortal, mainCamera.transform.rotation);

        portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        var clipThroughSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * targetPortal.vectorPlane;

        var obliqueProjectionMatrix = mainCamera.CalculateObliqueMatrix(clipThroughSpace);
        portalCamera.projectionMatrix = obliqueProjectionMatrix;

        portalCamera.Render();
    }

    void OnDestroy()
    {
        viewThroughRenderTexture.Release();

        Destroy(viewThroughMaterial);
        Destroy(viewThroughRenderTexture);
    }

    public static Vector3 TransformPositionBetweenPortals(Portal sender, Portal target, Vector3 position)
    {
        return target.normalInvisible.TransformPoint(sender.normalVisible.InverseTransformPoint(position));
    }

    public static Quaternion TransformRotationBetweenPortals(Portal sender, Portal target, Quaternion rotation)
    {
        return target.normalInvisible.rotation * Quaternion.Inverse(sender.normalVisible.rotation) * rotation;
    }
}
