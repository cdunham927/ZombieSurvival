using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class ShadowEffect : MonoBehaviour
    {
        [Header("Avoid cut corners by setting sprite Mesh Type to Full Rect.")]

        public Vector2 offset = Vector2.zero;

        public Color color = Color.white;

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

            mpb.SetVector("_ShadowOffset", enable ? offset : Vector2.zero);
            mpb.SetColor("_ShadowColor", enable ? color : Color.white);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
