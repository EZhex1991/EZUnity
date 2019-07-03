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
			#pragma shader_feature _DEBUGMODE_NORMAL _DEBUGMODE_TANGENT _DEBUGMODE_BITANGENT _DEBUGMODE_UV0 _DEBUGMODE_UV1
			#pragma shader_feature _WRAPMODE_CLAMP _WRAPMODE_REPEAT

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

#if _DEBUGMODE_NORMAL
					color.rgb = i.worldNormal;
#elif _DEBUGMODE_TANGENT
					color.rgb = i.worldTangent;
#elif _DEBUGMODE_BITANGENT
					color.rgb = i.worldBitangent;
#elif _DEBUGMODE_UV0
					color.rg = i.uv0;
#elif _DEBUGMODE_UV1
					color.rg = i.uv1;
#endif

#if _WRAPMODE_CLAMP
					color.rg = clamp(color.rg, 0, 1);
#elif _WRAPMODE_REPEAT
					color.rg = fmod(color.rg, 1);
#endif

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
