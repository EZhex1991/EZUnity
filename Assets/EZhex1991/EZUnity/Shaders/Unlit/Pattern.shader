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
		[KeywordEnum(ChessBoard, Diamond, Frame, Spot, Stripe, Triangle, Wave, Diagonal)]
		_PatternType ("Pattern Type", Float) = 0
		[KeywordEnum(UV, LocalPos_XY, LocalPos_XZ, LocalPos_YZ, ScreenPos)]
		_CoordMode ("Coordinate Mode", Float) = 0
		_SecondColor ("Second Color", Color) = (0, 0, 0, 1)
		[EZVectorSingleLine] _ScaleOffset ("Scale(XY) Offset(ZW)", Vector) = (2, 2, 0, 0)
		[EZVectorSingleLine] _PatternCenter ("Center", Vector) = (0.5, 0.5, 0, 0)
		_FillRatio ("Fill Ratio", Range(0, 1)) = 0.5

		[Header(Distrotion)]
		[KeywordEnum(None, Rotate, Swirl, Shrink)]
		_DistortionType ("Distortion Type", Float) = 0
		_Rotation ("Rotation", float) = 3.14
		_Swirl ("Swirl", float) = 3.14
		_Shrink ("Shrink", float) = 1
	}
	CustomEditor "EZhex1991.EZUnity.EZUnlitPatternShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _COORDMODE_UV _COORDMODE_LOCALPOS_XY _COORDMODE_LOCALPOS_XZ _COORDMODE_LOCALPOS_YZ _COORDMODE_SCREENPOS
			#pragma shader_feature _PATTERNTYPE_CHESSBOARD _PATTERNTYPE_DIAMOND _PATTERNTYPE_FRAME _PATTERNTYPE_SPOT _PATTERNTYPE_STRIPE _PATTERNTYPE_TRIANGLE _PATTERNTYPE_WAVE _PATTERNTYPE_DIAGONAL
			#pragma shader_feature _ _DISTORTIONTYPE_ROTATE _DISTORTIONTYPE_SWIRL _DISTORTIONTYPE_SHRINK

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			fixed4 _SecondColor;
			fixed4 _ScaleOffset;
			fixed4 _PatternCenter;
			fixed _FillRatio;

			fixed _Rotation;
			fixed _Swirl;
			fixed _Shrink;

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
				float2 coord = i.patternPos * _ScaleOffset.xy + _ScaleOffset.zw;
								
				#if _PATTERNTYPE_DIAMOND
					coord = frac(coord) - _PatternCenter.xy;
				#elif _PATTERNTYPE_FRAME
					coord = (frac(coord) - _PatternCenter.xy) * 2;
				#else
					coord = frac(coord) - _PatternCenter.xy;
				#endif
				
				#if _DISTORTIONTYPE_ROTATE
					float sinValue, cosValue;
					sincos(_Rotation, sinValue, cosValue);
					float2x2 rotationMatrix = float2x2(cosValue, -sinValue, sinValue, cosValue);
					coord = mul(coord, rotationMatrix);
				#elif _DISTORTIONTYPE_SWIRL
					float distanceToCenter = length(coord);
					float sinValue, cosValue;
					sincos(distanceToCenter * _Swirl, sinValue, cosValue);
					float2x2 rotationMatrix = float2x2(cosValue, -sinValue, sinValue, cosValue);
					coord = mul(coord, rotationMatrix);
				#elif _DISTORTIONTYPE_SHRINK
					float distanceToCenter = length(coord);
					coord = coord * pow(distanceToCenter * 2, _Shrink);
				#endif

				#ifdef _PATTERNTYPE_CHESSBOARD
					color *= lerp(_SecondColor, _Color, step(0, coord.x * coord.y));
				#elif _PATTERNTYPE_DIAMOND
					color *= lerp(_SecondColor, _Color, step(_FillRatio, abs(coord.x) + abs(coord.y)));
				#elif _PATTERNTYPE_FRAME
					color *= lerp(_SecondColor, _Color, saturate(step(_FillRatio, abs(coord.x)) + step(_FillRatio, abs(coord.y))));
				#elif _PATTERNTYPE_SPOT
					color *= lerp(_SecondColor, _Color, step(_FillRatio * 0.707, length(coord)));
				#elif _PATTERNTYPE_STRIPE
					color *= lerp(_SecondColor, _Color, step(_FillRatio, frac(coord.x + coord.y)));
				#elif _PATTERNTYPE_TRIANGLE
					color *= lerp(_SecondColor, _Color, step(coord.y, coord.x - (_FillRatio - 0.5) * 2));
				#elif _PATTERNTYPE_WAVE
					color *= lerp(_SecondColor, _Color, step(sin(coord.x * 6.2832) * 0.5, coord.y - (_FillRatio - 0.5) * 2));
				#elif _PATTERNTYPE_DIAGONAL
					color *= lerp(_SecondColor, _Color, step(abs(coord.x) * _FillRatio, abs(coord.y) * (1 - _FillRatio)));
				#endif

				return color;
			}

			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
