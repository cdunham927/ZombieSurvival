Shader "Hidden/NegativeScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		[Toggle] _Invert("Invert", Float) = 0
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

			half _Invert;

			float4 frag(v2f_img i) : COLOR
			{
				half4 c = tex2D(_MainTex, i.uv);

				c.rgb = _Invert == 1 ? 1 - c.rgb : c.rgb;

				return c;
			}

		ENDCG

		}
	}
}
