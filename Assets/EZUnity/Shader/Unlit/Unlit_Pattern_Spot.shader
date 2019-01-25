// Author:			ezhex1991@outlook.com
// CreateTime:		2019-01-16 14:02:40
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Pattern/Spot" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_SecondColor ("Second Color", Color) = (0, 0, 0, 1)
		[KeywordEnum(UV, LocalXY, LocalXZ, LocalYZ, ScreenPos)] _PatternMode ("Pattern Mode", Int) = 0
		_DensityFactor ("Density(XY) Offset(ZW)", Vector) = (2, 2, 0, 0)
		_FillRatio ("Fill Ratio", Range(0, 1)) = 0.5
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
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float2 patternPos: TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _SecondColor;
			int _PatternMode;
			fixed4 _DensityFactor;
			float _FillRatio;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				if (_PatternMode == 0) {
					o.patternPos = o.mainUV;
				} else if (_PatternMode == 1) {
					o.patternPos = v.vertex.xy;
				} else if (_PatternMode == 2) {
					o.patternPos = v.vertex.xz;
				} else if (_PatternMode == 3) {
					o.patternPos = v.vertex.yz;
				} else {
					o.patternPos = ComputeScreenPos(o.pos);
				}
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.mainUV);
				float2 coord = frac(i.patternPos * _DensityFactor.xy + _DensityFactor.zw) - 0.5;
				if (length(coord) < _FillRatio * 0.707) {
					color *= _Color;
				} else {
					color *= _SecondColor;
				}
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
