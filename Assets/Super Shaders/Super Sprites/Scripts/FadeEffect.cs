using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class FadeEffect : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float fade = 0f;

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

            mpb.SetFloat("_Fade", enable ? fade : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
