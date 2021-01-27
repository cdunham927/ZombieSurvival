Shader "Super/ColorSwapShader"
{
	Properties
	{ 
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[Header(Color Swap Properties)]
		_SourceA("Source A", Color) = (0,0,1,1)
		_TargetA("Target A", Color) = (1,0,0,1)
		_SourceB("Source B", Color) = (0,0,1,1)
		_TargetB("Target B", Color) = (1,0,0,1)
		_SourceC("Source C", Color) = (0,0,1,1)
		_TargetC("Target C", Color) = (1,0,0,1)

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

			float4 _SourceA;
			float4 _TargetA;
			float4 _SourceB;
			float4 _TargetB;
			float4 _SourceC;
			float4 _TargetC;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				// color swap a
				if (c.r >= _SourceA.r - 0.005 && c.r <= _SourceA.r + 0.005 && c.g >= _SourceA.g - 0.005 &&
					c.g <= _SourceA.g + 0.005 && c.b >= _SourceA.b - 0.005 && c.b <= _SourceA.b + 0.005)
				{
					c.rgb = _TargetA.rgb;
				}

				// color swap b
				if (c.r >= _SourceB.r - 0.005 && c.r <= _SourceB.r + 0.005 && c.g >= _SourceB.g - 0.005 &&
					c.g <= _SourceB.g + 0.005 && c.b >= _SourceB.b - 0.005 && c.b <= _SourceB.b + 0.005)
				{
					c.rgb = _TargetB.rgb;
				}

				// color swap c
				if (c.r >= _SourceC.r - 0.005 && c.r <= _SourceC.r + 0.005 && c.g >= _SourceC.g - 0.005 &&
					c.g <= _SourceC.g + 0.005 && c.b >= _SourceC.b - 0.005 && c.b <= _SourceC.b + 0.005)
				{
					c.rgb = _TargetC.rgb;
				}

				c.rgb *= c.a;

				return c;
			}

			ENDCG

		}
	}

	Fallback "Sprites/Default"
}