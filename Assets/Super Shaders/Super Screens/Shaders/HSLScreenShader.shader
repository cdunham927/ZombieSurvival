Shader "Hidden/HSLScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_Hue("Hue", Range(-1, 1)) = 0
		_Saturation("Saturation", Range(-1, 1)) = 0
		_Light("Light", Range(-1, 1)) = 0
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;

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

			float4 frag(v2f_img i) : COLOR
			{
				half4 c = tex2D(_MainTex, i.uv); 
				
				float3 adj = float3(_Hue, _Saturation, _Light);

				float3 hsl = RGBtoHSL(c.rgb);
				float mx = step(0, hsl.r) * step(hsl.r, 1);

				c.rgb = HSLtoRGB(hsl + adj.xyz * mx);

				return c;
			}

		ENDCG

		}
	}
}
