Shader "Hidden/SepiaScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_Sepia("Sepia", Range(0, 1)) = 0
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

			half _Sepia;

			float4 frag(v2f_img i) : COLOR 
			{
				half4 c = tex2D(_MainTex, i.uv); 
				
				fixed3 sep;

				sep.r = dot(c.rgb, half3(0.39, 0.77, 0.19));
				sep.g = dot(c.rgb, half3(0.35, 0.69, 0.17));
				sep.b = dot(c.rgb, half3(0.27, 0.53, 0.13));

				c.rgb = lerp(c.rgb, sep, _Sepia);

				return c;
			}

		ENDCG

		}
	}
}
