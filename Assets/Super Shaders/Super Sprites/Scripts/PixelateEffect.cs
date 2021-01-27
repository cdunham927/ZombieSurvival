using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class PixelateEffect : MonoBehaviour
    {
        public float pixelSize = 0.001f;

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

            mpb.SetFloat("_PixelSize", enable ? pixelSize : 0.001f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
