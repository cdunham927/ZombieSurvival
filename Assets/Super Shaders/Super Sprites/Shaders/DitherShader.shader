Shader "Super/DitherShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		
		_DitherTex("Dither Texture", 2D) = "white" {}
		_DitherColorA("Dither Color A", Color) = (1, 1, 1, 1)
		_DitherColorB("Dither Color B", Color) = (1, 1, 1, 1)
		_DitherScale("Dither Scale", Range(0, 0.1)) = 0.001
		_DitherStrength("Dither Strength", Range(0, 1)) = 1

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

			sampler2D _DitherTex;
			float4 _DitherColorA;
			float4 _DitherColorB;
			half _DitherScale;
			half _DitherStrength;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				half v = tex2D(_DitherTex, round(IN.texcoord.xy / _DitherScale) * _DitherScale).r;

				c *= lerp(_DitherColorA, _DitherColorB, max(1 - _DitherStrength, step(v, c.r)));

				return c;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}