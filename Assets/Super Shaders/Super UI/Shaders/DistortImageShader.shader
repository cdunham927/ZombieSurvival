﻿Shader "Super/UI/DistortShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		[Space]
		_DistortTex("Distort Texture", 2D) = "bump" {}
		_HorizontalDistort("Horizontal Distort", Range(-0.1, 0.1)) = 0.1
		_VerticalDistort("Vertical Distort", Range(-0.1, 0.1)) = 0.1
		_HorizontalScale("Horizontal Scale", Float) = 1
		_VerticalScale("Vertical Scale", Float) = 1

		[Space]
		[Enum(UnityEngine.Rendering.CompareFunction)]
		_StencilComp("Stencil Comparison", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)]
		_StencilOp("Stencil Operation", Float) = 0
		_Stencil("Stencil ID", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
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

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
				#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;
				float4 _MainTex_ST; 
				
				sampler2D _DistortTex;
				float _HorizontalDistort;
				float _VerticalDistort;
				float _HorizontalScale;
				float _VerticalScale;

				v2f vert(appdata_t v)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
					OUT.worldPosition = v.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

					OUT.color = v.color * _Color;
					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{ 
					float2 s = float2(_HorizontalScale, _VerticalScale);
					float2 d = float2(_HorizontalDistort, _VerticalDistort);

					half4 color = (tex2D(_MainTex, IN.texcoord + (d * tex2D(_DistortTex, s * IN.texcoord))) + _TextureSampleAdd) * IN.color; //  + (d * tex2D(_DistortTex, s * IN.texcoord))

					#ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					#endif

					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif

					return color;
				}
			ENDCG
			}
		}
}