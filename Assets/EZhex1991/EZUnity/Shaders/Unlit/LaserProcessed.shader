// Author:			ezhex1991@outlook.com
// CreateTime:		2019-05-15 11:13:11
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/LaserProcessed" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		_LaserRamp ("Laser Ramp", 2D) = "white" {}
		_FlowMap("Flow Map", 2D) = "grey" {}
		_FlowMapStrength ("Flow Map Strength", Range(-1, 1)) = 0.2
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

			sampler2D _LaserRamp;
			sampler2D _FlowMap;
			float4 _FlowMap_ST;
			fixed _FlowMapStrength;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv_FlowMap : TEXCOORD1;
				float3 worldViewDir : TEXCOORD3;
				float3 worldNormalDir : TEXCOORD4;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.uv_FlowMap = TRANSFORM_TEX(v.uv0, _FlowMap);
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.worldNormalDir = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				float dotNV = dot(i.worldNormalDir, i.worldViewDir);

				fixed flow = (tex2D(_FlowMap, i.uv_FlowMap).r - 0.5) * _FlowMapStrength;
				float2 laserUV = float2(1 - dotNV + flow, 0);

				half4 laserColor = tex2D(_LaserRamp, laserUV);
				color.rgb *= laserColor.rgb;

				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
