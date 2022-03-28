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

    private Camera mainCamera;
}
