using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenTintEffect : MonoBehaviour
    {
        public Color tint = Color.white;

        public enum Blend
        {
            None, Add, Multiply, Subtract
        }
        public Blend blend = Blend.None;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/TintScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetColor("_Tint", tint);

            material.SetFloat("_Blend", (float)blend);

            Graphics.Blit(source, destination, material);
        }
    }
}