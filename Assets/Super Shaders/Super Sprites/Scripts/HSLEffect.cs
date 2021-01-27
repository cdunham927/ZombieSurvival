using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class HSLEffect : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float hue = 0f;

        [Range(-1f, 1f)]
        public float saturation = 0f;

        [Range(-1f, 1f)]
        public new float light = 0f;

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

            mpb.SetFloat("_Hue", enable ? hue : 0f);
            mpb.SetFloat("_Saturation", enable ? saturation : 0f);
            mpb.SetFloat("_Light", enable ? light : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
