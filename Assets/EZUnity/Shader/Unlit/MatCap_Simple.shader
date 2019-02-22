// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-20 19:21:48
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/MatCap/Simple" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[NonScaleOffset]
		_MatcapTex ("Matcap Texture", 2D) = "white" {}
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
				float3 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float2 matcapUV : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			sampler2D _MatcapTex;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				o.matcapUV = mul(UNITY_MATRIX_IT_MV, v.normal);
				o.matcapUV = o.matcapUV * 0.5 + 0.5;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;

				half4 matcapColor = tex2D(_MatcapTex, i.matcapUV);
				color = color * matcapColor;

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
