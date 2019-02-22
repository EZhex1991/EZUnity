// Author:			ezhex1991@outlook.com
// CreateTime:		2018-09-18 15:34:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/_Debug" {
	Properties {
		[KeywordEnum(Normal, Tangent, Bitangent, UV0, UV1)]
		_DebugMode ("Debug Mode", Int) = 0
		[KeywordEnum(Clamp, Repeat)]
		_WrapMode ("Wrap Mode", Int) = 0
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
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldTangent : TEXCOORD3;
				float3 worldBitangent : TEXCOORD4;
			};

			int _DebugMode;
			int _WrapMode;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
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
					color.rg = i.uv0;
				} else if (_DebugMode == 4) {
					color.rg = i.uv1;
				}

				if (_WrapMode == 0) {
					color.rg = clamp(color.rg, 0, 1);
				} else if (_WrapMode == 1) {
					color.rg = fmod(color.rg, 1);
				}

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
