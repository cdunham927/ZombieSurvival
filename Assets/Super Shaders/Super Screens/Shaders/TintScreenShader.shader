Shader "Hidden/TintScreenShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}

		_Tint("Tint", Color) = (1, 1, 1, 1)

		[KeywordEnum(None, Add, Multiply, Subtract)] _Blend("Blend Mode", Float) = 0
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

			fixed4 _Tint;

			half _Blend;

			float4 frag(v2f_img i) : COLOR 
			{
				half4 c = tex2D(_MainTex, i.uv);

				return (_Blend == 1) ? c + _Tint : (_Blend == 2) ? c * _Tint : (_Blend == 3) ? c - _Tint : c;
			}

		ENDCG

		}
	}
}
