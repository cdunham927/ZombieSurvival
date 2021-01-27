Shader "Hidden/DistortScreenShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_DistortTex("Distort Texture", 2D) = "bump" {}

		_HorizontalDistort("Horizontal Distort", Range(-0.1, 0.1)) = 0.1
		_VerticalDistort("Vertical Distort", Range(-0.1, 0.1)) = 0.1

		_HorizontalScale("Horizontal Scale", Float) = 1
		_VerticalScale("Vertical Scale", Float) = 1
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

			sampler2D _DistortTex;
			half _HorizontalDistort;
			half _VerticalDistort;
			half _HorizontalScale;
			half _VerticalScale;

			fixed4 frag(v2f_img i) : SV_Target
			{
				half2 s = half2(_HorizontalScale, _VerticalScale);
				half2 d = half2(_HorizontalDistort, _VerticalDistort);

				return (tex2D(_MainTex, i.uv + (d * tex2D(_DistortTex, s * i.uv))));
			}

		ENDCG

		}
	}
}