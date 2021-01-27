using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class BlurEffect : MonoBehaviour
    {
        [Range(0f, 60f)]
        public float blur = 15f;

        [Range(150f, 900f)]
        public float focus = 350f;

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

            mpb.SetFloat("_Blur", enable ? blur : 15f);
            mpb.SetFloat("_Focus", enable ? focus : 350f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
