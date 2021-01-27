using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenHSLEffect : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float hue = 0f;

        [Range(-1f, 1f)]
        public float saturation = 0f;

        [Range(-1f, 1f)]
        public new float light = 0f;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/HSLScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetFloat("_Hue", hue);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Light", light);

            Graphics.Blit(source, destination, material);
        }
    }
}