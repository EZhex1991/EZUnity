// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dynamic/AlphaMask" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Alpha)]
		_AlphaMask ("Alpha Mask (R)", 2D) = "white" {}
		_Speed ("Speed(XY)", Vector) = (0.2, 0.2, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			sampler2D _AlphaMask;
			fixed4 _Speed;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv) * _Color;
				color.a = tex2D(_AlphaMask, i.uv + frac(_Speed * _Time.y));
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
