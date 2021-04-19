// Author:			ezhex1991@outlook.com
// CreateTime:		2020-11-30 11:41:31
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/EZHolographicProjection" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)
		_Exposure ("Exposure", Range (0, 8)) = 1

		[Header(Rim)]
		[HDR] _RimColor ("Rim Color", Color) = (0.3, 0.8, 0.3, 1)
		[PowerSlider(8)] _RimPower ("Rim Power", Range(0.1, 128)) = 2

		[Header(Noise1)]
		[NoScaleOffset] _Noise1Tex ("Noise Texture", 2D) = "white" {}
		_Noise1UVSpeed ("Noise UV(XY) Speed(ZW)", Vector) = (0, 1, 0, 0.6)
		
		[Header(Noise2)]
		[NoScaleOffset] _Noise2Tex ("Noise Texture", 2D) = "white" {}
		_Noise2UVSpeed ("Noise UV(XY) Speed(ZW)", Vector) = (0, 2, 0, 0.3)

		_Cutoff ("Cutoff", Range(0, 1)) = 0.15
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;
			fixed _Exposure;

			half4 _RimColor;
			fixed _RimPower;

			sampler2D _Noise1Tex;
			fixed4 _Noise1UVSpeed;
			
			sampler2D _Noise2Tex;
			fixed4 _Noise2UVSpeed;

			fixed _Cutoff;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
				float2 uv_MainTex : TEXCOORD3;
				float2 uv_Noise1Tex : TEXCOORD4;
				float2 uv_Noise2Tex : TEXCOORD5;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.uv_Noise1Tex = o.worldPos.xy * _Noise1UVSpeed.xy + _Noise1UVSpeed.zw * _Time.y;
				o.uv_Noise2Tex = o.worldPos.xy * _Noise2UVSpeed.xy + _Noise2UVSpeed.zw * _Time.y;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				float dotNV = dot(i.worldNormal, i.worldViewDir);
				fixed rim = pow((1.0 - max(0, dotNV)), _RimPower);
				color.rgb += _RimColor * rim;

				fixed noise1 = tex2D(_Noise1Tex, i.uv_Noise1Tex);
				fixed noise2 = tex2D(_Noise2Tex, i.uv_Noise2Tex);
				clip(noise1 * noise2 - _Cutoff);
				return color * _Exposure;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
