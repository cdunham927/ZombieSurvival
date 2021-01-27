Shader "Super/ShadowShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[Header(Avoid cut corners by setting sprite Mesh Type to Full Rect.)]
		_ShadowColor("Color", Color) = (1, 1, 1, 1)
		_ShadowOffset("Offset", Vector) = (0, 0, 0, 0)

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

			fixed4 _ShadowOffset;
			fixed4 _ShadowColor;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 s = SampleSpriteTexture(IN.texcoord + _ShadowOffset) * IN.color;
				s.rgb = _ShadowColor;
				s.a *= _ShadowColor.a;
				
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				fixed4 o;

				o.rgb = (s.rgb * (1 - c.a)) + (c.rgb * c.a);
				o.a = min(s.a + c.a, 1);

				return o;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}