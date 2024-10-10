using UnityEngine;

[ExecuteInEditMode]
public class PixelationEffect : MonoBehaviour
{
    public Material pixelationMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelationMaterial != null)
        {
            Graphics.Blit(src, dest, pixelationMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
