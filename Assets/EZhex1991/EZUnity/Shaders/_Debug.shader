// Author:			ezhex1991@outlook.com
// CreateTime:		2018-09-18 15:34:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/_Debug" {
	Properties {
		_DebugTex ("Debug Texture", 2D) = "white" {}
		[KeywordEnum(Normal, Tangent, Bitangent, Color, UV0, UV1)]
		_DebugMode ("Debug Mode", Int) = 0
		[KeywordEnum(Clamp, Repeat, Mirror)]
		_WrapMode ("Wrap Mode", Int) = 0

		_DebugColor ("Debug Color", Color) = (1, 1, 1, 1)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _DEBUGMODE_NORMAL _DEBUGMODE_TANGENT _DEBUGMODE_BITANGENT _DEBUGMODE_COLOR _DEBUGMODE_UV0 _DEBUGMODE_UV1
			#pragma shader_feature _WRAPMODE_CLAMP _WRAPMODE_REPEAT _WRAPMODE_MIRROR

			#include "UnityCG.cginc"

			sampler2D _DebugTex;
			float4 _DebugTex_ST;
			half4 _DebugColor;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldTangent : TEXCOORD3;
				float3 worldBitangent : TEXCOORD4;
				float4 color : TEXCOORD5;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = TRANSFORM_TEX(v.uv0, _DebugTex);
				o.uv1 = TRANSFORM_TEX(v.uv1, _DebugTex);
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				o.worldBitangent = normalize(cross(o.worldNormal, o.worldTangent));
				o.color = v.color;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = 0;

				#if _DEBUGMODE_NORMAL
					color.rgb = i.worldNormal * 0.5 + 0.5;
				#elif _DEBUGMODE_TANGENT
					color.rgb = i.worldTangent * 0.5 + 0.5;
				#elif _DEBUGMODE_BITANGENT
					color.rgb = i.worldBitangent * 0.5 + 0.5;
				#elif _DEBUGMODE_UV0
					color.rg = i.uv0;
				#elif _DEBUGMODE_UV1
					color.rg = i.uv1;
				#elif _DEBUGMODE_COLOR
					color = i.color;
				#endif

				#if _WRAPMODE_CLAMP
					color = clamp(color, 0, 1);
				#elif _WRAPMODE_REPEAT
					color = fmod(color, 1);
				#elif _WRAPMODE_MIRROR
					color = abs(fmod(color + 1, 2) - 1);
				#endif

				return color * tex2D(_DebugTex, i.uv0) * _DebugColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
