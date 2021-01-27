Shader "Super/UI/HSLShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		[Space]
		_Hue("Hue", Range(-1, 1)) = 0
		_Saturation("Saturation", Range(-1, 1)) = 0
		_Light("Light", Range(-1, 1)) = 0

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

				half _Hue;
				half _Saturation;
				half _Light;

				float3 RGBtoHCV(in float3 RGB)
				{
					// Based on work by Sam Hocevar and Emil Persson
					float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
					float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
					float C = Q.x - min(Q.w, Q.y);
					float H = abs((Q.w - Q.y) / (6 * C + 1e-10) + Q.z);
					return float3(H, C, Q.x);
				}

				float3 HUEtoRGB(in float H)
				{
					float R = abs(H * 6 - 3) - 1;
					float G = 2 - abs(H * 6 - 2);
					float B = 2 - abs(H * 6 - 4);
					return saturate(float3(R, G, B));
				}

				float3 HSLtoRGB(in float3 HSL)
				{
					float3 RGB = HUEtoRGB(HSL.x);
					float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
					return (RGB - 0.5) * C + HSL.z;
				}

				float3 RGBtoHSL(in float3 RGB)
				{
					float3 HCV = RGBtoHCV(RGB);
					float L = HCV.z - HCV.y * 0.5;
					float S = HCV.y / (1 - abs(L * 2 - 1) + 1e-10);
					return float3(HCV.x, S, L);
				}

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
					half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

					#ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					#endif

					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif

					float3 adj = float3(_Hue, _Saturation, _Light);

					float3 hsl = RGBtoHSL(color.rgb);
					float mx = step(0, hsl.r) * step(hsl.r, 1);

					color.rgb = HSLtoRGB(hsl + adj.xyz * mx);

					return color;
				}
			ENDCG
			}
		}
}