// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-22 19:33:10
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Matcap" {
	Properties {
		[Header(Base)]
		[EZTextureMini(_Color)] _MainTex ("Main Texture", 2D) = "white" {}
		[HideInInspector][HDR] _Color ("Color", Color) = (1, 1, 1, 1)
		
		[EZTextureKeyword(_BUMP_ON)]
		[EZTextureSingleLine(_Bumpiness)] _BumpTex ("Bump Texture", 2D) = "bump" {}
		[HideInInspector] _Bumpiness ("Bumpiness", Range(0, 2)) = 1
		
		[EZTextureSingleLine(_DiffColor)] _DiffMatcap ("Diffuse Matcap", 2D) = "white" {}
		[HideInInspector][HDR] _DiffColor ("Diffuse Color (RGB, Strength)", Color) = (1, 1, 1, 1)
		
		[EZTextureKeyword(_SPEC_ON)]
		[EZTextureSingleLine(_SpecColor)] _SpecMatcap ("Specular Matcap", 2D) = "black" {}
		[HideInInspector][HDR] _SpecColor ("Specular Color (RGB, Strength)", Color) = (1, 1, 1, 1)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ _BUMP_ON
			#pragma shader_feature _ _SPEC_ON

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;

#if _BUMP_ON
			sampler2D _BumpTex;
			fixed _Bumpiness;
#endif			

			sampler2D _DiffMatcap;
			half4 _DiffColor;

#if _SPEC_ON
			sampler2D _SpecMatcap;
			half4 _SpecColor;
#endif

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
#if _BUMP_ON
				float3 worldNormal : TEXCOORD2;
				float3 tbnSpace0 : TEXCOORD10;
				float3 tbnSpace1 : TEXCOORD11;
				float3 tbnSpace2 : TEXCOORD12;
#else
				float2 matcapUV : TEXCOORD1;
#endif
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = v.uv0;
#if _BUMP_ON
				float3 worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				float3 worldBitangent = cross(o.worldNormal, worldTangent) * tangentSign;
				o.tbnSpace0 = float3(worldTangent.x, worldBitangent.x, o.worldNormal.x);
				o.tbnSpace1 = float3(worldTangent.y, worldBitangent.y, o.worldNormal.y);
				o.tbnSpace2 = float3(worldTangent.z, worldBitangent.z, o.worldNormal.z);
#else
				o.matcapUV = mul(UNITY_MATRIX_IT_MV, v.normal) * 0.5 + 0.5;
#endif
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;

#if _BUMP_ON
				float3 bumpTex = UnpackNormal(tex2D(_BumpTex, i.mainUV));
				float3 normal;
				normal.x = dot(i.tbnSpace0, bumpTex);
				normal.y = dot(i.tbnSpace1, bumpTex);
				normal.z = dot(i.tbnSpace2, bumpTex);
				normal = lerp(i.worldNormal, normal, _Bumpiness);
				float2 matcapUV = mul(UNITY_MATRIX_IT_MV, normal) * 0.5 + 0.5;
#else
				float2 matcapUV = i.matcapUV;
#endif

				half4 diff = tex2D(_DiffMatcap, matcapUV) * _DiffColor;
				color *= diff;

#if _SPEC_ON
				half4 spec = tex2D(_SpecMatcap, matcapUV) * _SpecColor;
				color += spec;
#endif

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
