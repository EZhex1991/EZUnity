// Author:			ezhex1991@outlook.com
// CreateTime:		2018-12-11 10:53:49
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Surface/Diffuse" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
