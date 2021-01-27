Shader "Super/StencilShader"
{
	Properties
	{ 
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[Header(Stencil Properties)]
		_Stencil("Stencil", Float) = 5

		[Enum(UnityEngine.Rendering.CompareFunction)]
		_Comparison("Comparison", Float) = 0

		[Enum(UnityEngine.Rendering.StencilOp)]
		_Operation("Operation", Float) = 0

		_WriteMask("Write Mask", Range(0, 255)) = 255
		_ReadMask("Read Mask", Range(0, 255)) = 255
		_AlphaCutoff("Alpha Cutoff", Range(0.01, 1.0)) = 0.01

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
			Stencil
			{
				Ref [_Stencil]
				Comp [_Comparison]
				Pass [_Operation]
				ReadMask [_ReadMask]
				WriteMask [_WriteMask]
			}

			CGPROGRAM

			#pragma vertex SpriteVert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnitySprites.cginc"
			#include "UnityShaderVariables.cginc"

			fixed _AlphaCutoff;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				clip(c.a - _AlphaCutoff);

				return c;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}