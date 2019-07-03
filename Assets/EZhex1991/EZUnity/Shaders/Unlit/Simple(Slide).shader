// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Simple(Slide)" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Slide)]
		_SlideSpeed ("Speed(XY) Scale(ZW)", Vector) = (20, 20, 0.01, 0.01)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			fixed4 _SlideSpeed;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 uv = v.uv + frac(_SlideSpeed.xy * _SlideSpeed.zw * _Time.y);
				o.uv_MainTex = TRANSFORM_TEX(uv, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
