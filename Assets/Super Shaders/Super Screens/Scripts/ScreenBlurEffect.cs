using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenBlurEffect : MonoBehaviour
    {
        [Range(0f, 60f)]
        public float blur = 4f;

        [Range(150f, 900f)]
        public float focus = 400f;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/BlurScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }
            
            material.SetFloat("_Blur", blur);
            material.SetFloat("_Focus", focus);

            Graphics.Blit(source, destination, material);
        }
    }
}