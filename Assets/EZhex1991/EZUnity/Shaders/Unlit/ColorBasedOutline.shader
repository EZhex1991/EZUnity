// Author:			ezhex1991@outlook.com
// CreateTime:		2019-08-23 15:27:28
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/ColorBasedOutline" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_OutlineTolerance ("Outline Tolerance", Float) = 50
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 0)
		_OutlineThickness ("Outline Thickness", Float) = 1
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
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			fixed _OutlineTolerance;
			half4 _OutlineColor;
			int _OutlineThickness;

			float Difference (half3 rgb1, half3 rgb2) {
				return length(rgb1 - rgb2) * 255;
			}
			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv);
				int isBorder = 0;
				for (int x = -_OutlineThickness; x <= _OutlineThickness; x++){
					for (int y = -_OutlineThickness; y <= _OutlineThickness; y++){
						float2 uv = i.uv + float2(x, y) * _MainTex_TexelSize;
						isBorder += step(_OutlineTolerance, Difference(color, tex2D(_MainTex, uv)));
					}
				}
				color = lerp(color, _OutlineColor, saturate(isBorder));
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
