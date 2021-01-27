using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenNegativeEffect : MonoBehaviour
    {
        public bool invert = false;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/NegativeScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetFloat("_Invert", invert ? 1 : 0);

            Graphics.Blit(source, destination, material);
        }
    }
}