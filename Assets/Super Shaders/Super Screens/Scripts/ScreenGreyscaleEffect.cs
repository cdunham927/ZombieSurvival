using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenGreyscaleEffect : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float intensity = 0;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/GreyscaleScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetFloat("_Greyscale", intensity);

            Graphics.Blit(source, destination, material);
        }
    }
}