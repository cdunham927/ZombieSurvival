using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class ChromaticEffect : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float horizontalOffset = 0f;

        [Range(-1f, 1f)]
        public float verticalOffset = 0f;

        [Range(-1f, 1f)]
        public float strength = 0f;

        [Range(-1f, 1f)]
        public float offset = 0.25f;

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
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            spriteRenderer.GetPropertyBlock(mpb);

            mpb.SetFloat("_HorizontalChromaticOffset", enable ? horizontalOffset : 0f);
            mpb.SetFloat("_VerticalChromaticOffset", enable ? verticalOffset : 0f);
            mpb.SetFloat("_ChromaticStrength", enable ? strength : 0f);
            mpb.SetFloat("_ChromaticOffset", enable ? offset : 0.25f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
