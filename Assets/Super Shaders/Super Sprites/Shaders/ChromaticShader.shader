Shader "Super/ChromaticShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_HorizontalChromaticOffset("Horizontal Offset", Range(-1, 1)) = 0
		_VerticalChromaticOffset("Vertical Offset", Range(-1, 1)) = 0

		_ChromaticStrength("Strength", Range(-1, 1)) = 0
		_ChromaticOffset("Offset", Range(-1, 1)) = 0.25

		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM

				#pragma vertex SpriteVert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

				#include "UnitySprites.cginc"

				half _HorizontalChromaticOffset;
				half _VerticalChromaticOffset;
				half _ChromaticStrength;
				half _ChromaticOffset;

				fixed4 frag(v2f IN) : COLOR
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

					fixed2 d = fixed2(_HorizontalChromaticOffset, _VerticalChromaticOffset) * _ChromaticStrength;

					fixed4 r = SampleSpriteTexture(IN.texcoord + d * _ChromaticOffset) * IN.color;
					fixed4 g = SampleSpriteTexture(IN.texcoord + d * _ChromaticOffset * 2) * IN.color;
					fixed4 b = SampleSpriteTexture(IN.texcoord + d * _ChromaticOffset * 3) * IN.color;

					c.a = (r.a + g.a + b.a) / 3;

					c.r = r.r;
					c.g = g.g;
					c.b = b.b;

					c.rgb *= c.a;

					return c;
				}

			ENDCG
		}
	}

	Fallback "Sprites/Default"
}