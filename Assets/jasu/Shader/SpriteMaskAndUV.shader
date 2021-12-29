Shader "Custom/SpriteMaskAndUV"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_MaskTex("MaskTexture", 2D) = "white"{}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_ScrollX("ScrollX", Float) = 0.1
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
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 maskTexcoord : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float2 maskTexcoord : TEXCOORD1;
				};

				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.maskTexcoord = IN.maskTexcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _MaskTex;
				float _ScrollX;
				fixed4 frag(v2f IN) : SV_Target
				{
					IN.texcoord.x += _ScrollX * _Time;

					//fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					//c.rgb *= c.a;

					//return c;

					half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
					color.rgb *= color.a;

					half4 maskCol = tex2D(_MaskTex, IN.maskTexcoord);

					color.a *= maskCol.a;
					color.rgb *= maskCol.a;

					return color;
				}
			ENDCG
			}
		}
}
