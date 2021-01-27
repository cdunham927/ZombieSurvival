Shader "Super/ShiftShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[Header(Avoid strips by setting the sprites wrap mode to repeat.)]
		_HorizontalShift("Horizontal Shift", Float) = 0
		_VerticalShift("Vertical Shift", Float) = 0

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

			half _HorizontalShift;
			half _VerticalShift;

			fixed4 frag(v2f IN) : SV_Target
			{
				IN.texcoord.xy += float2(_HorizontalShift, _VerticalShift);

				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				c.rgb *= c.a;

				return c;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}