Shader "Hidden/BlurScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_Blur("Blur", Range(0, 60)) = 4
		_Focus("Focus", Range(150, 900)) = 400
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

			half _Blur;
			half _Focus;

			float4 frag(v2f_img i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv);

				half4 adj = half4(0, 0, 0, 0);

				half pixels = (_Blur / _Focus / 4) * 0.5;

				adj += tex2D(_MainTex, float2(i.uv.x - 3 * pixels, i.uv.y - 3 * pixels)) * 0.1;
				adj += tex2D(_MainTex, float2(i.uv.x + 3 * pixels, i.uv.y + 3 * pixels)) * 0.1;

				adj += tex2D(_MainTex, float2(i.uv.x - 2 * pixels, i.uv.y - 2 * pixels)) * 0.1;
				adj += tex2D(_MainTex, float2(i.uv.x + 2 * pixels, i.uv.y + 2 * pixels)) * 0.1;

				adj += tex2D(_MainTex, float2(i.uv.x - 1 * pixels, i.uv.y - 1 * pixels)) * 0.2;
				adj += tex2D(_MainTex, float2(i.uv.x + 1 * pixels, i.uv.y + 1 * pixels)) * 0.2;

				adj += tex2D(_MainTex, i.uv) * 0.2;

				return adj;
			}

		ENDCG

		}
	}
}