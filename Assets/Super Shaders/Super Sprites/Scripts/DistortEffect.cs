using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class DistortEffect : MonoBehaviour
    {
        public Texture2D distortTexture = null;

        [Range(-0.1f, 0.1f)]
        public float horizontalDistort = 0f;

        [Range(-0.1f, 0.1f)]
        public float verticalDistort = 0f;
        
        public Vector2 scale = Vector2.zero;

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
            if (distortTexture == null)
            {
                return;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            spriteRenderer.GetPropertyBlock(mpb);

            mpb.SetTexture("_DistortTex", distortTexture);

            mpb.SetFloat("_HorizontalDistort", enable ? horizontalDistort : 0f);
            mpb.SetFloat("_VerticalDistort", enable ? verticalDistort : 0f);

            mpb.SetFloat("_HorizontalScale", enable ? scale.x : 0f);
            mpb.SetFloat("_VerticalScale", enable ? scale.y : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
