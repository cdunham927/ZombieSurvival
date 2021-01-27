using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class DitherEffect : MonoBehaviour
    {
        public Texture2D ditherTexture = null;

        public Color colorA = Color.white;
        public Color colorB = Color.white;

        [Range(0f, 0.1f)]
        public float scale = 0.001f;

        [Range(0f, 1f)]
        public float strength = 1f;

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
            if (ditherTexture == null)
            {
                return;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            spriteRenderer.GetPropertyBlock(mpb);

            mpb.SetTexture("_DitherTex", ditherTexture);

            mpb.SetColor("_DitherColorA", enable ? colorA : Color.white);
            mpb.SetColor("_DitherColorB", enable ? colorB : Color.white);

            mpb.SetFloat("_DitherScale", enable ? scale : 0f);
            mpb.SetFloat("_DitherStrength", enable ? strength : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
