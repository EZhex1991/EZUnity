// Author:			熊哲
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dynamic/TextureFog" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Fog)]
		_FogTex ("Fog Texture (R)", 2D) = "black" {}
		_FogColor ("Fog Color", Color) = (1, 1, 1, 1)
		_Fog1 ("Fog1 Speed(XY) Strength(Z)", Vector) = (0.05, 0.01, 1.2, 0)
		_Fog2 ("Fog2 Speed(XY) Strength(Z)", Vector) = (0.1, 0, 0.8, 0)
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

			sampler2D _FogTex;
			fixed4 _FogColor;
			fixed4 _Fog1;
			fixed4 _Fog2;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv) * _Color;
				fixed fog1 = tex2D(_FogTex, i.uv + frac(_Fog1.xy * _Time.y)).r * _Fog1.z;
				fixed fog2 = tex2D(_FogTex, i.uv + frac(_Fog2.xy * _Time.y)).r * _Fog2.z;;
				fixed fog = (fog1 + fog2) / 2;
				color.rgb = lerp(color.rgb, _FogColor.rgb, fog);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
