// Author:			ezhex1991@outlook.com
// CreateTime:		2020-12-07 18:42:17
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/EZDissolve" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Noise)]
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		[HDR] _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
		_EdgeSize ("Edge Size", Range(0.0001, 1)) = 0.2
		_Cutoff ("Cutoff", Range(0, 1)) = 0.5
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" "RenderType" = "TransparentCutout" "IgnoreProjector" = "True" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			half4 _EdgeColor;
			fixed _EdgeSize;
			fixed _Cutoff;

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
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 noise = tex2D(_NoiseTex, i.uv_NoiseTex);
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				half cutoff = lerp(0, _Cutoff + _EdgeSize, _Cutoff);
				half4 edge = smoothstep(_Cutoff + _EdgeSize, _Cutoff, clamp(noise.r, _EdgeSize, 1));
				color += edge * _EdgeColor;
				
				clip(noise.r - cutoff);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
