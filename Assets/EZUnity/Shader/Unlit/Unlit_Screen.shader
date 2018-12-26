// Author:			熊哲
// CreateTime:		2018-12-11 14:59:08
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Screen" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.mainUV) * _Color;
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
