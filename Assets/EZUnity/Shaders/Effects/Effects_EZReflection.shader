// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-18 16:44:03
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Effects/EZReflection" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ReflectionTex ("Reflection Texture", 2D) = "" {}
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
				float4 screenPos : TEXCOORD01;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			sampler2D _ReflectionTex;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				o.screenPos = ComputeNonStereoScreenPos(o.pos);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;
				color *= tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(i.screenPos));
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
