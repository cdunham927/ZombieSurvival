Shader "Super/BlurShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_Blur("Blur", Range(0,60)) = 15
		_Focus("Focus", Range(150, 900)) = 350

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

			float _Blur;
			float _Focus;

			float4 frag(v2f IN) : COLOR
			{
				float4 adj = float4(0, 0, 0, 0);

				half pixels = (_Blur / _Focus / 4) * 0.5;

				adj += tex2D(_MainTex, float2(IN.texcoord.x - 3 * pixels, IN.texcoord.y - 3 * pixels)) * 0.1;
				adj += tex2D(_MainTex, float2(IN.texcoord.x + 3 * pixels, IN.texcoord.y + 3 * pixels)) * 0.1;

				adj += tex2D(_MainTex, float2(IN.texcoord.x - 2 * pixels, IN.texcoord.y - 2 * pixels)) * 0.1;
				adj += tex2D(_MainTex, float2(IN.texcoord.x + 2 * pixels, IN.texcoord.y + 2 * pixels)) * 0.1;

				adj += tex2D(_MainTex, float2(IN.texcoord.x - 1 * pixels, IN.texcoord.y - 1 * pixels)) * 0.2;
				adj += tex2D(_MainTex, float2(IN.texcoord.x + 1 * pixels, IN.texcoord.y + 1 * pixels)) * 0.2;

				adj += tex2D(_MainTex, float2(IN.texcoord.x, IN.texcoord.y)) * 0.2;

				adj.a *= IN.color.a;

				return adj;
			}

			ENDCG
		}
	}

	Fallback "Sprites/Default"
}