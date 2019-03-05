// Author:			ezhex1991@outlook.com
// CreateTime:		2018-12-11 14:59:08
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/SimpleBlack" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "black" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		
		// Rendering Mode Settings
		[HideInInspector] _RenderingMode ("Rendering Mode", Float) = 0
		[HideInInspector] _SrcBlendMode ("Source Blend", Float) = 1
		[HideInInspector] _DstBlendMode ("Destination Blend", Float) = 0
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		[HideInInspector] _ZWriteMode ("ZWrite", Float) = 1
		[HideInInspector] _CullMode ("Cull", Float) = 2
		[HideInInspector] _OffsetFactor ("Offset Factor", Float) = 0
		[HideInInspector] _OffsetUnit ("Offset Unit", Float) = 0
	}
	CustomEditor "EZShaderGUIWithRenderingModeSettings"
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

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			
			// Rendering Mode Settings
			int _RenderingMode;
			fixed _AlphaCutoff;

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
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.mainUV) * _Color;
				if (_RenderingMode == 1) {
					clip(color.a - _AlphaCutoff);
				}
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
