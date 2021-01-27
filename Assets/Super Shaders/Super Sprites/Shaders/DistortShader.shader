Shader "Super/DistortShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_DistortTex("Distort Texture", 2D) = "bump" {}

		_HorizontalDistort("Horizontal Distort", Range(-0.1, 0.1)) = 0.1
		_VerticalDistort("Vertical Distort", Range(-0.1, 0.1)) = 0.1

		_HorizontalScale("Horizontal Scale", Float) = 1
		_VerticalScale("Vertical Scale", Float) = 1

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

				sampler2D _DistortTex;

				float _HorizontalDistort;
				float _VerticalDistort;

				float _HorizontalScale;
				float _VerticalScale;

				fixed4 frag(v2f IN) : SV_Target
				{
					float2 s = float2(_HorizontalScale, _VerticalScale);
					float2 d = float2(_HorizontalDistort, _VerticalDistort);

					fixed4 c = SampleSpriteTexture(IN.texcoord + (d * tex2D(_DistortTex, s * IN.texcoord))) * IN.color;

					c.rgb *= c.a;

					return c;
				}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}