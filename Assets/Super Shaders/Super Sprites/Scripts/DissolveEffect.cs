using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class DissolveEffect : MonoBehaviour
    {
        public Texture2D dissolveTexture = null;

        public Color emissionColor = Color.white;

        [Range(0f, 1f)]
        public float amount = 1f;

        [Range(0f, 1f)]
        public float thickness = 0.15f;

        [Range(0f, 1f)]
        public float threshold = 0.1f;

        private SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            UpdateEffect(true);
        }

        private void OnDisable()
        {
            UpdateEffect(false);
        }

        private void Update()
        {
            UpdateEffect(true);
        }

        private void UpdateEffect(bool enable)
        {
            if (dissolveTexture == null)
            {
                return;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            spriteRenderer.GetPropertyBlock(mpb);

            mpb.SetTexture("_DissolveTexture", dissolveTexture);

            mpb.SetColor("_EmissionColor", enable ? emissionColor : Color.white);

            mpb.SetFloat("_DissolveAmount", enable ? amount : 1f);
            mpb.SetFloat("_EmissionThickness", enable ? thickness : 0.15f);
            mpb.SetFloat("_EmissionThreshold", enable ? threshold : 0.1f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
