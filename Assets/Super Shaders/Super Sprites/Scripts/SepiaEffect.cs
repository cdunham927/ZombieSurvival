using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class SepiaEffect : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float sepia = 0f;

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

            mpb.SetFloat("_Sepia", enable ? sepia : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
