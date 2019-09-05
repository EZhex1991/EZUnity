// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-02 17:45:16
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Distort_Twirl" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}

		[Header(Twirl)]
		_TwirlCenter ("Twirl Center", Vector) = (0.5, 0.5, 0, 0)
		_TwirlStrength ("Twirl Strength", Float) = 10
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;

			float4 _TwirlCenter;
			float _TwirlStrength;

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
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				float2 uv = i.uv_MainTex - _TwirlCenter;
				float angle = _TwirlStrength * length(uv);
				float x = cos(angle) * uv.x - sin(angle) * uv.y;
				float y = sin(angle) * uv.x + cos(angle) * uv.y;
				uv = float2(x + _TwirlCenter.x, y + _TwirlCenter.y);
				half4 color = tex2D(_MainTex, uv);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
