// Author:			ezhex1991@outlook.com
// CreateTime:		2019-01-16 14:02:40
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Pattern" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Pattern)]
		[KeywordEnum(ChessBoard, Diamond, Frame, Spot, Stripe, Triangle, Wave)]
		_PatternType ("Pattern Type", Float) = 0
		[KeywordEnum(UV, LocalPos_XY, LocalPos_XZ, LocalPos_YZ, ScreenPos)]
		_CoordMode ("Coordinate Mode", Float) = 0
		_SecondColor ("Second Color", Color) = (0, 0, 0, 1)
		_DensityFactor ("Density(XY) Offset(ZW)", Vector) = (2, 2, 0, 0)
		_FillRatio ("Fill Ratio", Range(0, 1)) = 0.5
	}
	CustomEditor "EZUnlitPatternShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _COORDMODE_UV _COORDMODE_LOCALPOS_XY _COORDMODE_LOCALPOS_XZ _COORDMODE_LOCALPOS_YZ _COORDMODE_SCREENPOS
			#pragma shader_feature _PATTERNTYPE_CHESSBOARD _PATTERNTYPE_DIAMOND _PATTERNTYPE_FRAME _PATTERNTYPE_SPOT _PATTERNTYPE_STRIPE _PATTERNTYPE_TRIANGLE _PATTERNTYPE_WAVE

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			fixed4 _SecondColor;
			fixed4 _DensityFactor;
			fixed _FillRatio;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float2 patternPos: TEXCOORD1;
			};
			
			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);

				#if _COORDMODE_UV
					o.patternPos = o.mainUV;
				#elif _COORDMODE_LOCALPOS_XY
					o.patternPos = v.vertex.xy;
				#elif _COORDMODE_LOCALPOS_XZ
					o.patternPos = v.vertex.xz;
				#elif _COORDMODE_LOCALPOS_YZ
					o.patternPos = v.vertex.yz;
				#elif _COORDMODE_SCREENPOS
					o.patternPos = ComputeScreenPos(o.pos);
				#endif

				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.mainUV);
				float2 coord = i.patternPos * _DensityFactor.xy + _DensityFactor.zw;

				#ifdef _PATTERNTYPE_CHESSBOARD
					coord = frac(coord) - 0.5;
					if (coord.x * coord.y > 0) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_DIAMOND
					coord = frac(coord) - 0.5;
					if (abs(coord.x) > abs(coord.y)) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_FRAME
					coord = frac(coord) * 2 - 1;
					if (abs(coord.x) < _FillRatio && abs(coord.y) < _FillRatio) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_SPOT
					coord = frac(coord) - 0.5;
					if (length(coord) < _FillRatio * 0.707) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_STRIPE
					if (frac(coord.x + coord.y) > _FillRatio) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_TRIANGLE
					coord = frac(coord) - 0.5;
					if (coord.x > coord.y) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#elif _PATTERNTYPE_WAVE
					coord = frac(coord) - 0.5;
					if (sin(coord.x * 6.2832) * 0.5 > coord.y) {
						color *= _Color;
					} else {
						color *= _SecondColor;
					}
				#endif

				return color;
			}

			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
