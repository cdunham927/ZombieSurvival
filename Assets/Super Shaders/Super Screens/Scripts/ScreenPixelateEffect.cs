using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenPixelateEffect : MonoBehaviour
    {
        public float intensity = 0f;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/PixelateScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetFloat("_PixelSize", intensity);

            Graphics.Blit(source, destination, material);
        }
    }
}