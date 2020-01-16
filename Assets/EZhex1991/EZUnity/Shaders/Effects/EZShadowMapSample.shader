// Author:			ezhex1991@outlook.com
// CreateTime:		2020-01-14 14:23:05
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/EZShadowMapSample" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		_ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
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

			half4 _ShadowColor;
			
			float3 _EZShadowMap_LightDirection;
			float4 _EZShadowMap_LightSplitsNear;
			float4 _EZShadowMap_LightSplitsFar;

			float4x4 _EZShadowMap_WorldToShadow[4];
			float4x4 _EZShadowMap_WorldToCamera;
			
			sampler2D _EZShadowMap_ShadowTex0;
			sampler2D _EZShadowMap_ShadowTex1;
			sampler2D _EZShadowMap_ShadowTex2;
			sampler2D _EZShadowMap_ShadowTex3;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float3 worldNormal : TEXCOORD2;
				float4 worldPos : TEXCOORD4;
			};

			inline fixed4 getCascadeWeights(float z) {
				fixed4 zNear = float4(z >= _EZShadowMap_LightSplitsNear);
				fixed4 zFar = float4(z < _EZShadowMap_LightSplitsFar);
				fixed4 weights = zNear * zFar;
				return weights;
			}

			float4 SampleShadowTexture (float4 worldPos, fixed4 cascadeWeights) {
				float4 shadowCoord0 = mul(_EZShadowMap_WorldToShadow[0], worldPos);
				float4 shadowCoord1 = mul(_EZShadowMap_WorldToShadow[1], worldPos);
				float4 shadowCoord2 = mul(_EZShadowMap_WorldToShadow[2], worldPos);
				float4 shadowCoord3 = mul(_EZShadowMap_WorldToShadow[3], worldPos);

				shadowCoord0.xy = (shadowCoord0.xy / shadowCoord0.w) * 0.5 + 0.5;
				shadowCoord1.xy = (shadowCoord1.xy / shadowCoord1.w) * 0.5 + 0.5;
				shadowCoord2.xy = (shadowCoord2.xy / shadowCoord2.w) * 0.5 + 0.5;
				shadowCoord3.xy = (shadowCoord3.xy / shadowCoord3.w) * 0.5 + 0.5;

				float4 sampleDepth0 = tex2D(_EZShadowMap_ShadowTex0, shadowCoord0);
				float4 sampleDepth1 = tex2D(_EZShadowMap_ShadowTex1, shadowCoord1);
				float4 sampleDepth2 = tex2D(_EZShadowMap_ShadowTex2, shadowCoord2);
				float4 sampleDepth3 = tex2D(_EZShadowMap_ShadowTex3, shadowCoord3);

				float depth0 = shadowCoord0.z / shadowCoord0.w;
				float depth1 = shadowCoord1.z / shadowCoord1.w;
				float depth2 = shadowCoord2.z / shadowCoord2.w;
				float depth3 = shadowCoord3.z / shadowCoord3.w;
				
				float shadow0 = sampleDepth0 < depth0 ? 0 : 1;
				float shadow1 = sampleDepth1 < depth1 ? 0 : 1;
				float shadow2 = sampleDepth2 < depth2 ? 0 : 1;
				float shadow3 = sampleDepth3 < depth3 ? 0 : 1;

				float shadow = shadow0 * cascadeWeights[0]
					+ shadow1 * cascadeWeights[1]
					+ shadow2 * cascadeWeights[2]
					+ shadow3 * cascadeWeights[3];
				return shadow;
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));

				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex);

				float dotNL = dot(i.worldNormal, _EZShadowMap_LightDirection);

				color *= lerp(_ShadowColor, 1, max(0, dotNL));
				
				float4 cascadeWeights = getCascadeWeights(-mul(_EZShadowMap_WorldToCamera, i.worldPos).z);
				float4 shadow = SampleShadowTexture(i.worldPos, cascadeWeights);
				color *= lerp(1, _ShadowColor, shadow * step(0, dotNL));

				return color;
			}
			ENDCG
		}
	}
}
