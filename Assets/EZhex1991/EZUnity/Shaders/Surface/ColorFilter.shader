// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Surface/ColorFilter"{
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Filter)]
		[KeywordEnum(Grey, HSV)] _FilterMode ("Filter Mode", Float) = 0
		_GreyFactor ("RGB Weight", Color) = (0.299, 0.587, 0.114, 1)
		_HSVFactor ("H(X) S(Y) V(Z) Scale(W)", Vector) = (0, 0, 0, 0.01)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0
		#pragma shader_feature _FILTERMODE_GREY _FILTERMODE_HSV

		#include "../CGIncludes/EZUnity.cginc"
		
		sampler2D _MainTex;
		half4 _Color;

		half4 _GreyFactor;
		half4 _HSVFactor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 mainTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
#if _FILTERMODE_GREY
			o.Albedo = dot(mainTex.rgb, _GreyFactor.rgb);
#elif _FILTERMODE_HSV
			half3 hsv = RGB2HSV(mainTex);
			hsv.x += _HSVFactor.x * _HSVFactor.w;
			hsv.y *= _HSVFactor.y * _HSVFactor.w + 1;
			hsv.z *= _HSVFactor.z * _HSVFactor.w + 1;
			if (hsv.x >= 1) hsv.x -= 1;
			o.Albedo = HSV2RGB(hsv);
#endif
			o.Alpha = mainTex.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}