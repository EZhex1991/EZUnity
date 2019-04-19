// Author:			ezhex1991@outlook.com
// CreateTime:		2018-12-11 14:59:08
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Advanced(Black)" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "black" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)
		
		// Rendering Settings
		[KeywordEnum(None, AlphaTest, AlphaBlend, AlphaPremultiply)] _AlphaMode ("Alpha Mode", Float) = 0
		_AlphaClipThreshold ("Alpha Clip Threshold", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendMode ("Src Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendMode ("Dst Blend Mode", Float) = 0
		[Toggle] _ZWriteMode ("ZWrite", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
		_OffsetFactor ("Offset Factor", Float) = 0
		_OffsetUnit ("Offset Unit", Float) = 0
	}
	CustomEditor "EZRenderingSettingsShaderGUI"
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

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;
			
			// Rendering Settings
			fixed _AlphaClipThreshold;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;
				#if _ALPHAMODE_ALPHATEST
					clip(color.a - _AlphaClipThreshold);
				#endif
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
