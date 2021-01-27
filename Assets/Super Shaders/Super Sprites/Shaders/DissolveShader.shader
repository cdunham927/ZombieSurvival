Shader "Super/DissolveShader"
{
	Properties
	{ 
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[Header(Dissolve Properties)]
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_EmissionColor("Emission Color", color) = (1, 1, 1, 1)
		_DissolveAmount("Dissolve Amount", Range(0, 1)) = 0
		_EmissionThickness("Emission Thickness", Range(0, 1)) = 0.15
		_EmissionThreshold("Emission Threshold", Range(0, 1)) = 0.1

		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#pragma vertex SpriteVert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnitySprites.cginc"

			sampler2D _DissolveTexture;
			fixed4 _EmissionColor;
			fixed _DissolveAmount; 
			fixed _EmissionThickness;
			fixed _EmissionThreshold;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				fixed4 b = fixed4(0, 0, 0, 0);

				half dv = tex2D(_DissolveTexture, IN.texcoord).r;

				if (dv < _DissolveAmount + _EmissionThickness)
				{
					half a = _DissolveAmount / _EmissionThreshold;

					b = fixed4(_EmissionColor.r, _EmissionColor.g, _EmissionColor.b, c.a * a);
				}

				if (dv <= _DissolveAmount)
				{
					b = c;
				}

				b.rgb *= _EmissionColor.a;

				return b;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}
