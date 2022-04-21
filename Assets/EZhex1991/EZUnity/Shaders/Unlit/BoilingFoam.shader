// Author:			ezhex1991@outlook.com
// CreateTime:		2022-04-21 11:09:12
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/BoilingFoam" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Noise)]
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Speed ("Speed", Float) = 0.1
		_WaveHeight("Wave Height", Range(0, 1)) = 0.2
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Transparency" }

		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			fixed _Speed;

			fixed _WaveHeight;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv_NoiseTex : TEXCOORD1;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.uv_NoiseTex = TRANSFORM_TEX(v.uv0, _NoiseTex);
				o.uv_NoiseTex.y = _Speed * _Time.y;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half noise = tex2D(_NoiseTex, i.uv_NoiseTex);
				i.uv_MainTex.y += (noise * 2 - 1) * _WaveHeight;
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
