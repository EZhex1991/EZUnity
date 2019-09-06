// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-06 10:11:52
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/ColorBasedOutline" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Tolerance)]
		_GrayWeight ("Gray Weight", Color) = (0.299, 0.587, 0.114, 1)
		_Tolerance ("Tolerance", Float) = 50

		[Header(Outline)]
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 0.5)
		_OutlineThickness ("Outline Thickness", Float) = 1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			half4 _Color;

			float4 _GrayWeight;
			float _Tolerance;

			half4 _OutlineColor;
			int _OutlineThickness;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			float RGBToGray (half3 rgb, half3 weight) {
				return dot(rgb, weight);
			}
			float Difference (half3 rgb1, half3 rgb2, half3 weight) {
				return abs(RGBToGray(rgb1, weight) - RGBToGray(rgb2, weight)) * 255;
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;
				int isBorder = 0;
				for (int x = -_OutlineThickness; x <= _OutlineThickness; x++) {
					for (int y = -_OutlineThickness; y <= _OutlineThickness; y++) {
						float2 uv = i.uv_MainTex + float2(x, y) * _MainTex_TexelSize;
						isBorder += step(_Tolerance, Difference(color, tex2D(_MainTex, uv) * _Color, _GrayWeight));
					}
				}
				color.rgb = lerp(color.rgb, _OutlineColor.rgb, saturate(isBorder) * _OutlineColor.a);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
