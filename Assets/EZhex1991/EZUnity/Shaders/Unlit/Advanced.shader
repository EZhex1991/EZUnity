// Author:			ezhex1991@outlook.com
// CreateTime:		2018-12-11 14:56:26
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Advanced" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)
		
		[Header(Rendering Settings)]
		[HideInInspector] _AlphaTex ("Alpha Tex (R)", 2D) = "white" {}
		[HideInInspector][KeywordEnum(None, AlphaTest, AlphaBlend, AlphaPremultiply)] _AlphaMode ("Alpha Mode", Float) = 0
		[HideInInspector] _AlphaClipThreshold ("Alpha Clip Threshold", Range(0, 1)) = 0.5
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendMode ("Src Blend Mode", Float) = 1
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlendMode ("Dst Blend Mode", Float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWriteMode ("ZWrite", Float) = 1
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
		[HideInInspector] _OffsetFactor ("Offset Factor", Float) = 0
		[HideInInspector] _OffsetUnit ("Offset Unit", Float) = 0
	} 
	CustomEditor "EZhex1991.EZUnity.EZRenderingSettingsShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			Blend [_SrcBlendMode] [_DstBlendMode]
			ZWrite [_ZWriteMode]
			Cull [_CullMode]
			Offset [_OffsetFactor], [_OffsetUnit]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma shader_feature _ _ALPHAMODE_ALPHATEST _ALPHAMODE_ALPHABLEND _ALPHAMODE_ALPHAPREMULTIPLY
			#pragma shader_feature _ _ALPHATEX_ON

			#include "UnityCG.cginc"
			
			// Rendering Settings
			sampler2D _AlphaTex;
			fixed _AlphaClipThreshold;
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				// Rendering Settings
				#if !_ALPHAMODE_NONE
					#if _ALPHATEX_ON
						color.a = tex2D(_AlphaTex, i.uv_MainTex).r;
					#endif
					#if _ALPHAMODE_ALPHATEST
						clip(color.a - _AlphaClipThreshold);
					#endif
				#else
					color.a = 1;
				#endif

				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
