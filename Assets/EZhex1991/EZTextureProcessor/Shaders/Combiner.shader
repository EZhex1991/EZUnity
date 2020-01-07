// Author:			ezhex1991@outlook.com
// CreateTime:		2020-01-07 11:08:14
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Combiner" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_AddTex ("Add Texture", 2D) = "white" {}
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
			sampler2D _AddTex;
			float4 _AddTex_ST;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv_AddTex : TEXCOORD1;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.uv_AddTex = TRANSFORM_TEX(v.uv0, _AddTex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 mainColor = tex2D(_MainTex, i.uv_MainTex);
				half4 addColor = tex2D(_AddTex, i.uv_AddTex);
				fixed s = step(0, i.uv_AddTex.x) * step(0, i.uv_AddTex.y) * step(i.uv_AddTex.x, 1) * step(i.uv_AddTex.y, 1);
				half4 color = lerp(mainColor, addColor, s);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
