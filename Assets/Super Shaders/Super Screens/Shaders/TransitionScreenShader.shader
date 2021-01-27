Shader "Hidden/TransitionScreenShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 0
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

			sampler2D _TransitionTex;
			fixed4 _Color;
			fixed _Cutoff;
			fixed _Fade;

			fixed4 frag(v2f_img i) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.uv);

				fixed4 t = tex2D(_TransitionTex, i.uv);

				return (t.b < _Cutoff) ? lerp(c, _Color, _Fade) : c;
			}

		ENDCG

		}
	}
}