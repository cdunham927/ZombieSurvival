using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class NegativeEffect : MonoBehaviour
    {
        public bool negative = false;

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

            mpb.SetFloat("_InvertColors", enable ? negative ? 1f : 0f : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
