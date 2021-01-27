using UnityEngine;

namespace SuperShaders.Screen
{
    [ExecuteInEditMode]
    public class ScreenTransitionEffect : MonoBehaviour
    {
        public Texture2D transition = null;

        public Color color = Color.white;

        [Range(0f, 1f)]
        public float cutoff = 0f;

        [Range(0f, 1f)]
        public float fade = 0f;

        private Material material;

        private void Awake()
        {
            material = new Material(Shader.Find("Hidden/TransitionScreenShader"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!material)
            {
                return;
            }

            material.SetTexture("_TransitionTex", transition);
            material.SetColor("_Color", color);
            material.SetFloat("_Cutoff", cutoff);
            material.SetFloat("_Fade", fade);

            Graphics.Blit(source, destination, material);
        }
    }
}