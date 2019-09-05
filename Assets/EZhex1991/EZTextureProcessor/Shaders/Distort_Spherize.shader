// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-02 16:30:27
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Distort_Spherize" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}

		[Header(Spherize)]
		_SpherizePower ("Spherize Power", Float) = 4
		_SpherizeCenter ("Spherize Center", Vector) = (0.5, 0.5, 0, 0)
		_SpherizeStrength ("Spherize Strength", Vector) = (10, 10, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;

			float _SpherizePower;
			float2 _SpherizeCenter;
			float2 _SpherizeStrength;

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
				float2 uv = i.uv_MainTex - _SpherizeCenter;
				float delta = pow(length(uv), _SpherizePower);
				uv = i.uv_MainTex + uv * _SpherizeStrength * delta;
				half4 color = tex2D(_MainTex, uv);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
