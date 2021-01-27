Shader "Hidden/GreyscaleScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_Greyscale("Greyscale", Range(0, 1)) = 0
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

			half _Greyscale;

			float4 frag(v2f_img i) : COLOR 
			{
				half4 c = tex2D(_MainTex, i.uv);

				c.rgb = lerp(c.rgb, dot(c.rgb, float3(0.3, 0.59, 0.11)), _Greyscale);

				return c;
			}

		ENDCG

		}
	}
}
