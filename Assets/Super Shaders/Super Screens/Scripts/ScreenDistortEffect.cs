using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenDistortEffect : MonoBehaviour
    {
        public Texture2D transition = null;

        [Range(-0.1f, 0.1f)]
        public float horizontalDistort = 0.1f;

        [Range(-0.1f, 0.1f)]
        public float verticalDistort = 0.1f;
        
        [Range(-10, 10)]
        public float horizontalScale = 1f;

        [Range(-10, 10)]
        public float verticalScale = 1f;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/DistortScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetTexture("_DistortTex", transition);

            material.SetFloat("_HorizontalDistort", horizontalDistort);
            material.SetFloat("_VerticalDistort", verticalDistort);

            material.SetFloat("_HorizontalScale", horizontalScale);
            material.SetFloat("_VerticalScale", verticalScale);

            Graphics.Blit(source, destination, material);
        }
    }
}