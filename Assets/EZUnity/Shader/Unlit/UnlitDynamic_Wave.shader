// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-26 10:55:01
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/UnlitDynamic/Wave" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)

		[Header(Wave)]
		_WaveTex ("Wave Texture", 2D) = "black" {}
		_Waves ("Waves(XY) Height(Z)", Vector) = (1, 1, 1, 1)
		_WaveSpeed ("Wave Speed Wave(XY) Tex(ZW)", Vector) = (0.2, 0.4, 0.3, 0.6)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			sampler2D _WaveTex;
			fixed4 _Waves;
			fixed4 _WaveSpeed;

			v2f vert (appdata v) {
				v2f o;
				float heightX = sin((v.uv.x * _Waves.x + _WaveSpeed.x * _Time.y) * 6.28);
				float heightY = sin((v.uv.y * _Waves.y + _WaveSpeed.y * _Time.y) * 6.28);
				v.vertex += v.normal * (heightX + heightY) * _Waves.z;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv) * _Color;
				fixed4 waveColor = tex2D(_WaveTex, i.uv + _WaveSpeed.zw * _Time.y);
				color += waveColor;
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
