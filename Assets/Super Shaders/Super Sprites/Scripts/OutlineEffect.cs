﻿using UnityEngine;

namespace SuperShaders
{
    [ExecuteInEditMode]
    public class OutlineEffect : MonoBehaviour
    {
        public Color color = Color.white;

        [Range(0, 16)]
        public int outlineSize = 1;

        private SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            UpdateOutline(true);
        }

        private void OnDisable()
        {
            UpdateOutline(false);
        }

        private void Update()
        {
            UpdateOutline(true);
        }

        private void UpdateOutline(bool enable)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            spriteRenderer.GetPropertyBlock(mpb);

            mpb.SetFloat("_Outline", enable ? 1f : 0);
            mpb.SetColor("_OutlineColor", color);
            mpb.SetFloat("_OutlineSize", outlineSize);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
