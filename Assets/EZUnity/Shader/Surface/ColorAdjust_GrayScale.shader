// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/ColorAdjust/GrayScale"{
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_RGBWeight ("RGB Weight", Color) = (0.299, 0.587, 0.114, 1)
	}
	SubShader {
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};
		
		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _RGBWeight;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed grey = dot(texColor.rgb, _RGBWeight.rgb);
			o.Albedo = fixed3(grey, grey, grey);
			o.Alpha = texColor.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}