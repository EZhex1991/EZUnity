// Author:			ezhex1991@outlook.com
// CreateTime:		2019-07-02 13:15:56
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/UVStreamer" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(UV Movements)]
		[EZVectorSingleLine] _UVMovements ("UV Movements", Vector) = (1, 1, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Blend SrcAlpha One
		ZWrite Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			fixed4 _UVMovements;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);

				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				float3 worldBitangent = cross(worldNormal, worldTangent);
				float3 worldView = normalize(WorldSpaceViewDir(v.vertex));

				float2 offset = float2(dot(worldView, worldTangent), dot(worldView, worldBitangent));
				o.uv_MainTex += offset * _UVMovements.xy + offset * _UVMovements.wz;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;				
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
