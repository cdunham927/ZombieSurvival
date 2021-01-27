Shader "Hidden/PixelateScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_PixelSize("Pixel Size", Float) = 0
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

			float _PixelSize;

			float4 frag(v2f_img i) : COLOR
			{
				half r = (_PixelSize + 0.01) * 0.001;

				float2 size = float2(r, r);

				float2 uv = i.uv.xy;
				uv /= size;
				uv = round(uv);
				uv *= size;

				half4 c = tex2D(_MainTex, uv);

				return c;
			}

		ENDCG

		}
	}
}
