// Author:			ezhex1991@outlook.com
// CreateTime:		2018-12-11 14:56:26
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Simple" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		// EZShaderGUI Properties
		[HideInInspector] _RenderingMode ("_RenderingMode", Float) = 0
		[HideInInspector] _SrcBlendMode ("_SrcBlendMode", Float) = 1
		[HideInInspector] _DstBlendMode ("_DstBlendMode", Float) = 0
		[HideInInspector] _AlphaCutoff ("_AlphaCutoff", Range(0, 1)) = 0.5
		[HideInInspector] _ZWriteMode ("_ZWriteMode", Float) = 1
		[HideInInspector] _CullMode ("_CullMode", Float) = 2
		[HideInInspector] _OffsetFactor ("_OffsetFactor", Float) = 0
		[HideInInspector] _OffsetUnit ("_OffsetUnit", Float) = 0
	}
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
			
			// EZShaderGUI Properties
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
	CustomEditor "EZShaderGUI"
}
