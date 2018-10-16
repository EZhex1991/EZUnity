// Author:			熊哲
// CreateTime:		2018-09-18 15:34:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Debug" {
	Properties {
		[KeywordEnum(Normal, Tangent, Bitangent, UV)]
		_DebugMode ("Debug Mode", Int) = 0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 worldTangent : TANGENT;
				float3 worldBitangent : TEXCOORD1;
			};

			int _DebugMode;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				o.worldBitangent = normalize(cross(o.worldNormal, o.worldTangent));
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = 1;
				if (_DebugMode == 0) {
					color.rgb = i.worldNormal;
				} else if (_DebugMode == 1) {
					color.rgb = i.worldTangent;
				} else if (_DebugMode == 2) {
					color.rgb = i.worldBitangent;
				} else if (_DebugMode == 3) {
					color.rg = i.uv;
				}
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
