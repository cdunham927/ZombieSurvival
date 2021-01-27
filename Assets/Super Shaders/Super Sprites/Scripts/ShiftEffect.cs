using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class ShiftEffect : MonoBehaviour
    {
        public Vector2 shift = Vector2.zero;

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

            mpb.SetFloat("_HorizontalShift", enable ? shift.x : 0f);
            mpb.SetFloat("_VerticalShift", enable ? shift.y : 0f);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
